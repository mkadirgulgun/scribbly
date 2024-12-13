using NoteTakingApp.Models;

namespace NoteTakingApp.ViewModels
{
    public class SearchResultViewModel
    {
        public string SearchTerm { get; set; }
        public List<Note> Notes { get; set; }
        public List<Tag> UniqueTags { get; set; }
    }
}
