using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace book_management.DTOs
{
    public class UpdateBookRequestDTO
    {
        public string Title { get; set; } = String.Empty;
        public int PublicationYear { get; set; }
        public string AuthorName { get; set; } = String.Empty;
    }
}