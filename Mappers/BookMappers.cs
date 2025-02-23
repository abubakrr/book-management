using book_management.DTOs;
using book_management.Models;

namespace book_management.Mappers
{
    public static class BookMappers
    {
        public static BookDTO toDTO(this Book book)
        {
            return new BookDTO
            {
                Id = book.Id,
                Title = book.Title,
                PublicationYear = book.PublicationYear,
                AuthorName = book.AuthorName,
                ViewCount = book.ViewCount
            };
        }

        public static Book toDomain(this CreateBookRequestDTO dto)
        {
            return new Book
            {
                Title = dto.Title,
                PublicationYear = dto.PublicationYear,
                AuthorName = dto.AuthorName
            };
        }
    }
}