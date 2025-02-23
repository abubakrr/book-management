namespace book_management.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; } = String.Empty;
        public int ViewCount { get; set; }
    }
}