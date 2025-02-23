using book_management.Data;
using book_management.DTOs;
using book_management.Helpers;
using book_management.Interfaces;
using book_management.Models;
using Microsoft.EntityFrameworkCore;

namespace book_management.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDBContext _context;

        public BookRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetAllAsync(QueryObject query)
        {
            var queryable = _context.Books.AsQueryable();
            if (!string.IsNullOrEmpty(query.Query))
            {
                queryable = queryable.Where(b => b.Title.ToLower().Contains(query.Query.ToLower()) || b.AuthorName.ToLower().Contains(query.Query.ToLower()));
            }
            return await queryable
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (book != null)
            {
                book.ViewCount++;
                await _context.SaveChangesAsync();
            }
            return book;
        }

        public async Task<List<Book>> GetByIdsAsync(List<int> ids)
        {
            return await _context.Books
                   .Where(b => ids.Contains(b.Id))
                   .ToListAsync();
        }

        public async Task<Book?> GetByTitleAsync(string title)
        {
            return await _context.Books.FirstOrDefaultAsync(b => b.Title.ToLower() == title.ToLower());
        }

        public async Task<List<Book>> GetByTitlesAsync(List<string> titles)
        {
            return await _context.Books
                .Where(b => titles.Contains(b.Title))
                .ToListAsync();
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return book;
        }


        public async Task<List<Book>> CreateBulkAsync(List<Book> books)
        {
            await _context.Books.AddRangeAsync(books);
            await _context.SaveChangesAsync();
            return books;
        }

        public async Task<Book?> UpdateAsync(int id, UpdateBookRequestDTO book)
        {
            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);
            if (bookToUpdate == null)
            {
                return null;
            }
            else
            {
                bookToUpdate.Title = book.Title;
                bookToUpdate.AuthorName = book.AuthorName;
                bookToUpdate.PublicationYear = book.PublicationYear;


                await _context.SaveChangesAsync();
                return bookToUpdate;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = _context.Books.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return false;
            }
            else
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> DeleteBulkAsync(List<int> ids)
        {
            var booksToDelete = await _context.Books.Where(b => ids.Contains(b.Id)).ToListAsync();
            if (booksToDelete.Count == 0)
            {
                return false;
            }

            _context.Books.RemoveRange(booksToDelete);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}