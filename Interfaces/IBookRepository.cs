using book_management.DTOs;
using book_management.Helpers;
using book_management.Models;

namespace book_management.Interfaces
{
    public interface IBookRepository
    {
        Task<List<Book>> GetAllAsync(QueryObject query);
        Task<Book?> GetByIdAsync(int id);
        Task<List<Book>> GetByIdsAsync(List<int> ids);
        Task<Book?> GetByTitleAsync(string title);
        Task<List<Book>> GetByTitlesAsync(List<string> title);
        Task<Book> CreateAsync(Book book);
        Task<List<Book>> CreateBulkAsync(List<Book> books);
        Task<Book?> UpdateAsync(int id, UpdateBookRequestDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> DeleteBulkAsync(List<int> ids);
    }
}