namespace NoteTakingApp.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string UserId { get; set; }
        public AppUser User { get; set; }

        public ICollection<NoteTag> NoteTags { get; set; }
    }
}
