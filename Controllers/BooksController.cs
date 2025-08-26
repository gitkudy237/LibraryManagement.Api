using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Models;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BooksController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetBook([FromRoute] int id)
        {
            var book = await _context.Books.FindAsync(id);

            return book is null ?
                NotFound() : Ok(book.ToBookDto());
        }

        [HttpGet]
        public async Task<ActionResult<List<BookDto>>> GetBooks()
        {
            var books = await _context.Books.ToListAsync();
            var result = books.Select(b => b.ToBookDto());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var book = createBookDto.ToBookModel();

            await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();

            return Ok(book.ToBookDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBook([FromRoute] int id, [FromBody] UpdateBookDto updateBookDto)
        {
            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook is null)
                return NotFound("Attempt to update unexisting book");

            existingBook.MapUpdateBook(updateBookDto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBook([FromRoute] int id)
        {
            var existingBook = await _context.Books.FindAsync(id);

            if (existingBook is null)
                return NotFound();

            _context.Books.Remove(existingBook);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
