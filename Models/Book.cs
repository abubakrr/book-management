namespace book_management.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; } = String.Empty;
        public int ViewCount { get; set; } = 0;
    }
}