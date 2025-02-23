using book_management.Models;
using Microsoft.EntityFrameworkCore;

namespace book_management.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(
             DbContextOptions<ApplicationDBContext> options
         ) : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}