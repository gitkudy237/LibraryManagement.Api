using LibraryManagement.Core.Abstractions;
using LibraryManagement.Dtos.BookDtos;
using LibraryManagement.Dtos.QueryObjectDto;
using LibraryManagement.Dtos.QueryResultDtos;
using LibraryManagement.Extensions;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public BooksController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetBook([FromRoute] int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);

            return book is null ?
                NotFound() : Ok(book.ToBookDto());
        }

        [HttpGet]
        public async Task<ActionResult<BookQueryResultDto>> GetBooks([FromQuery] BookQueryObjectDto bookQueryObjDto)
        {

            var bookQueryObj = bookQueryObjDto.ToBookQueryObjectModel();
            var queryResult = await _unitOfWork.Books.GetAll(bookQueryObj);

            var result = queryResult.ToBookQueryResultDto();

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var book = createBookDto.ToBookModel();

            await _unitOfWork.Books.AddAsync(book);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book.ToBookDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBook([FromRoute] int id, [FromBody] UpdateBookDto updateBookDto)
        {
            var existingBook = await _unitOfWork.Books.GetByIdAsync(id);

            if (existingBook is null)
                return NotFound("Attempt to update unexisting book");

            existingBook.MapUpdateBook(updateBookDto);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBook([FromRoute] int id)
        {
            var existingBook = await _unitOfWork.Books.GetByIdAsync(id, includeRelated: true);

            if (existingBook is null)
                return NotFound();
            if (existingBook.Borrowings.Any(b => b.ReturnDate == null))
                return BadRequest("Cannot delete actively borrowed book");

            _unitOfWork.Books.Delete(existingBook);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
