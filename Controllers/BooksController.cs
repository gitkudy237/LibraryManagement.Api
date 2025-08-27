using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Dtos.QueryObjectDto;
using LibraryManagement.Mappings;
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
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookQueryObjectDto bookQueryObjDto)
        {
            var query = _context.Books
                .Include(b => b.Author)
                .AsQueryable();

            var bookQueryObj = bookQueryObjDto.ToBookQueryObjectModel();

            // filtering
            if (!string.IsNullOrWhiteSpace(bookQueryObj.Title))
                query = query.Where(b => b.Title.Contains(bookQueryObj.Title));

            if (!string.IsNullOrWhiteSpace(bookQueryObj.AuthorName))
                query = query.Where(b => b.Author.Name.Contains(bookQueryObj.AuthorName));

            // sorting
            if (!string.IsNullOrWhiteSpace(bookQueryObj.SortBy))
            {
                if (bookQueryObj.SortBy.Equals("Title", StringComparison.CurrentCultureIgnoreCase))
                    query = bookQueryObj.IsSortAscending ?
                        query.OrderBy(b => b.Title) : query.OrderByDescending(b => b.Title);
            }

            // paging
            if ((bookQueryObj.PageNumber > 0  && bookQueryObj.PageSize > 0))
                query = query
                    .Skip((bookQueryObj.PageNumber - 1) * bookQueryObj.PageSize)
                    .Take(bookQueryObj.PageSize);


            var queryResult = await query.ToListAsync();
            var result = queryResult.Select(b => b.ToBookDto());

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
