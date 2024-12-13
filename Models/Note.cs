using Microsoft.AspNetCore.Identity;

namespace NoteTakingApp.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public bool IsArchive { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<NoteTag> NoteTags { get; set; }
    }
    
}
