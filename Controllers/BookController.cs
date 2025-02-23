using book_management.DTOs;
using book_management.Helpers;
using book_management.Interfaces;
using book_management.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace book_management.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _repository;

        public BookController(IBookRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves a list of books with pagination.
        /// </summary>
        /// <param name="queryObject">Query parameters for filtering and sorting books.</param>
        /// <returns>A list of books.</returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] QueryObject queryObject)
        {
            var books = await _repository.GetAllAsync(query: queryObject);
            var bookDetails = books.Select(b => new
            {
                b.Id,
                b.Title,
                PopularityScore = (b.ViewCount * 0.5) + ((DateTime.Now.Year - b.PublicationYear) * 2)
            })
            .OrderByDescending(b => b.PopularityScore)
            .Select(b => new
            {
                Id = b.Id,
                Title = b.Title
            });

            return Ok(bookDetails);
        }

        /// <summary>
        /// Retrieves a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book.</param>
        /// <returns>The book with the specified ID.</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var book = await _repository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book.toDTO());
        }

        /// <summary>
        /// Creates multiple books in bulk.
        /// </summary>
        /// <param name="dtos">A list of book creation requests.</param>
        /// <returns>The created books.</returns>
        [HttpPost("create-bulk")]
        public async Task<IActionResult> CreateBulk([FromBody] IEnumerable<CreateBookRequestDTO> dtos)
        {
            var existingBooks = await _repository.GetByTitlesAsync(dtos.Select(dto => dto.Title).ToList());
            if (existingBooks.Any())
            {
                var existingTitles = existingBooks.Select(b => b.Title);
                return Conflict(new { message = "Some books with the same titles already exist.", titles = existingTitles });
            }

            var books = dtos.Select(dto => dto.toDomain()).ToList();
            await _repository.CreateBulkAsync(books);
            var bookDTOs = books.Select(b => b.toDTO());

            return Ok(bookDTOs);
        }

        /// <summary>
        /// Creates a new book.
        /// </summary>
        /// <param name="dto">The book creation request.</param>
        /// <returns>The created book.</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookRequestDTO dto)
        {
            var existingBook = await _repository.GetByTitleAsync(dto.Title);
            if (existingBook != null)
            {
                return Conflict(new { message = "A book with the same title already exists." });
            }

            var book = dto.toDomain();
            await _repository.CreateAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book.toDTO());
        }

        /// <summary>
        /// Updates an existing book.
        /// </summary>
        /// <param name="id">The ID of the book to update.</param>
        /// <param name="dto">The book update request.</param>
        /// <returns>The updated book.</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateBookRequestDTO dto)
        {
            var book = await _repository.UpdateAsync(id, dto);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book.toDTO());
        }

        /// <summary>
        /// Deletes a book by its ID.
        /// </summary>
        /// <param name="id">The ID of the book to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes multiple books in bulk.
        /// </summary>
        /// <param name="ids">A list of book IDs to delete.</param>
        /// <returns>No content if the deletion was successful.</returns>
        [HttpDelete("delete-bulk")]
        public async Task<IActionResult> DeleteBulk([FromBody] IEnumerable<int> ids)
        {
            if (!ids.Any())
            {
                return BadRequest(new { message = "The list of book IDs cannot be empty." });
            }

            var books = await _repository.GetByIdsAsync([.. ids]);
            if (!books.Any())
            {
                var notFoundIds = ids.Except(books.Select(b => b.Id)).ToList();
                return NotFound(new { message = "Some books with given ids were not found.", ids = notFoundIds });
            }

            var result = await _repository.DeleteBulkAsync([.. ids]);
            if (!result)
            {
                return StatusCode(500, new { message = "Something went wrong while deleting the books." });
            }

            return NoContent();
        }
    }
}