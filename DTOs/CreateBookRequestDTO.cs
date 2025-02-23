namespace book_management.DTOs
{
    public class CreateBookRequestDTO
    {
        public string Title { get; set; } = String.Empty;
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; } = String.Empty;
    }
}