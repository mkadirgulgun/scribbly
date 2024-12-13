using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteTakingApp.Data;
using NoteTakingApp.Models;
using NoteTakingApp.ViewModels;
using System.Security.Claims;
using System.Web;

namespace NoteTakingApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        public NoteController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        [Route("notes")]
        public async Task<IActionResult> HomePage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notes = await _context.Notes
        .Where(x => x.IsArchive == false && x.IsDeleted == false && x.UserId == userId)
        .Include(x => x.NoteTags)
        .ThenInclude(nt => nt.Tag)
        .OrderByDescending(x => x.UpdatedTime)
        .ToListAsync();

            // Etiketleri gruplayarak tekil hale getir
            var uniqueTags = notes
                .SelectMany(n => n.NoteTags)
                .Select(nt => nt.Tag)
                .GroupBy(t => t.Name) // Etiket adlarına göre gruplama
                .Select(g => g.First()) // Her gruptan ilk etiketi seçme
                .ToList();

            // Görünüm modeline notlar ve benzersiz etiketleri ekle
            var viewModel = new HomePageViewModel
            {
                Notes = notes,
                UniqueTags = uniqueTags
            };

            return View(viewModel);
        }
        [Authorize]
        [Route("archive")]
        public async Task<IActionResult> ArchivePage()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var notes = await _context.Notes
        .Where(x => x.IsArchive == true && x.IsDeleted == false && x.UserId == userId)
        .Include(x => x.NoteTags)
        .ThenInclude(nt => nt.Tag)
        .OrderByDescending(x => x.UpdatedTime)
        .ToListAsync();

            // Etiketleri gruplayarak tekil hale getir
            var uniqueTags = notes
                .SelectMany(n => n.NoteTags)
                .Select(nt => nt.Tag)
                .GroupBy(t => t.Name) // Etiket adlarına göre gruplama
                .Select(g => g.First()) // Her gruptan ilk etiketi seçme
                .ToList();

            // Görünüm modeline notlar ve benzersiz etiketleri ekle
            var viewModel = new HomePageViewModel
            {
                Notes = notes,
                UniqueTags = uniqueTags
            };

            return View(viewModel);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddNote(Note note, string Tags, string returnUrl = null)
        {
            if (note == null || string.IsNullOrWhiteSpace(note.Title) || string.IsNullOrWhiteSpace(note.Detail))
            {
                ModelState.AddModelError("", "Note, title, and detail are required.");
                return View(note);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return Unauthorized();
            }

            note.UserId = userId;
            note.User = user;
            note.CreatedTime = DateTime.Now;
            note.UpdatedTime = DateTime.Now;
            note.IsArchive = note.IsArchive;
            note.IsDeleted = false;

            // Save the detail as HTML without encoding
            note.Detail = note.Detail;

            _context.Notes.Add(note);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrWhiteSpace(Tags))
            {
                var tagNames = Tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                               .Select(t => t.Trim())
                               .Where(t => !string.IsNullOrEmpty(t));

                foreach (var tagName in tagNames)
                {
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName && t.UserId == userId);
                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Name = tagName,
                            UserId = userId,
                            User = user
                        };
                        _context.Tags.Add(tag);
                        await _context.SaveChangesAsync();
                    }

                    var noteTag = new NoteTag
                    {
                        NoteId = note.Id,
                        TagId = tag.Id
                    };
                    _context.NoteTags.Add(noteTag);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction($"{returnUrl}");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UpdateNote(int id, string title, string detail, string tags, string returnUrl)
        {
            var note = await _context.Notes
                .Include(n => n.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id);

            if (note == null)
            {
                return NotFound();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            note.Title = title;
            note.Detail = detail;
            note.UpdatedTime = DateTime.Now;

            if (tags == null)
            {
                ViewData["FailMessage"] = "Etiket girmediniz";
                return RedirectToAction($"{returnUrl}");
            }
            var tagList = tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                              .Select(t => t.Trim())
                              .Distinct()
                              .ToList();

            var tagsToRemove = note.NoteTags.Where(nt => !tagList.Contains(nt.Tag.Name)).ToList();
            foreach (var tagToRemove in tagsToRemove)
            {
                note.NoteTags.Remove(tagToRemove);
            }

            foreach (var tagName in tagList)
            {
                if (!note.NoteTags.Any(nt => nt.Tag.Name == tagName))
                {
                    var tag = await _context.Tags.FirstOrDefaultAsync(t => t.Name == tagName);
                    if (tag == null)
                    {
                        tag = new Tag
                        {
                            Name = tagName,
                            UserId = userId
                        };
                        _context.Tags.Add(tag);
                    }
                    note.NoteTags.Add(new NoteTag { Note = note, Tag = tag });
                }
            }

            await _context.SaveChangesAsync();

            return RedirectToAction($"{returnUrl}");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ArchiveNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            note.IsArchive = true;
            note.UpdatedTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("HomePage");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RestoreNote(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            note.IsArchive = false;
            note.UpdatedTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction("ArchivePage");
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> DeleteNote(int id, string returnUrl)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note == null)
            {
                return NotFound();
            }

            note.IsDeleted = true;
            note.UpdatedTime = DateTime.Now;

            await _context.SaveChangesAsync();

            return RedirectToAction($"{returnUrl}");
        }
        [HttpGet("Search")]
        public IActionResult Search(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var searchResults = _context.Notes
                .Where(n => n.IsDeleted == false && n.UserId == userId &&
                    (n.Title.Contains(searchTerm) ||
                     n.Detail.Contains(searchTerm) ||
                     n.NoteTags.Any(nt => nt.Tag.Name.Contains(searchTerm))))
                .Include(n => n.NoteTags)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(n => n.UpdatedTime)
                .ToList();

            var viewModel = new SearchResultViewModel
            {
                SearchTerm = searchTerm,
                Notes = searchResults,
                UniqueTags = _context.Tags
                    .Where(t => t.NoteTags.Any(nt => nt.Note.UserId == userId && nt.Tag.IsDeleted == false))
                    .ToList()
            };

            return View(viewModel);
        }

        [HttpGet("{noteType}/{id:int}")]
        public async Task<IActionResult> ViewNote(string noteType, int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isArchive = noteType.Equals("Archive", StringComparison.OrdinalIgnoreCase);

            var note = await _context.Notes
                .Include(x => x.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.Id == id && n.UserId == userId && !n.IsDeleted && n.IsArchive == isArchive);

            if (note == null)
            {
                return NotFound();
            }

            var notes = await _context.Notes
                .Where(x => x.IsArchive == isArchive && x.IsDeleted == false && x.UserId == userId)
                .Include(x => x.NoteTags)
                .ThenInclude(nt => nt.Tag)
                .OrderByDescending(x => x.UpdatedTime)
                .ToListAsync();

            var uniqueTags = notes
                .SelectMany(n => n.NoteTags)
                .Select(nt => nt.Tag)
                .GroupBy(t => t.Name)
                .Select(g => g.First())
                .ToList();

            var viewModel = new HomePageViewModel
            {
                Notes = notes,
                UniqueTags = uniqueTags
            };

            return View(isArchive ? "ArchivePage" : "HomePage", viewModel);
        }
    }
}
