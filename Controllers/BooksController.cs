using LibraryManagement.Core.Abstractions;
using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Dtos.QueryObjectDto;
using LibraryManagement.Extensions;
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
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IBookRepository bookRepository,
         IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetBook([FromRoute] int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);

            return book is null ?
                NotFound() : Ok(book.ToBookDto());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks([FromQuery] BookQueryObjectDto bookQueryObjDto)
        {

            var bookQueryObj = bookQueryObjDto.ToBookQueryObjectModel();
            var queryResult = await _bookRepository.GetAll(bookQueryObj);

            var result = queryResult.Items.Select(qr => qr.ToBookDto());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var book = createBookDto.ToBookModel();

            await _bookRepository.AddAsync(book);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book.ToBookDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBook([FromRoute] int id, [FromBody] UpdateBookDto updateBookDto)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);

            if (existingBook is null)
                return NotFound("Attempt to update unexisting book");

            existingBook.MapUpdateBook(updateBookDto);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBook([FromRoute] int id)
        {
            var existingBook = await _bookRepository.GetByIdAsync(id);

            if (existingBook is null)
                return NotFound();

            _bookRepository.Delete(existingBook);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
