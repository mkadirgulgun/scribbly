using NoteTakingApp.Models;

namespace NoteTakingApp.ViewModels
{
    public class HomePageViewModel
    {
        public List<Note> Notes { get; set; }
        public List<Tag> UniqueTags { get; set; }
    }

}
