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
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BookDto>> GetBook([FromRoute] int id)
        {
            var result = await _bookService.GetBookByIdAsync(id);

            return result is null ?
                NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<BookQueryResultDto>> GetBooks([FromQuery] BookQueryObjectDto bookQueryObjDto)
        {
            var result = await _bookService.GetAllBooksAsync(bookQueryObjDto);

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<BookDto>> AddBook([FromBody] CreateBookDto createBookDto)
        {
            var result = await _bookService.AddBookAsync(createBookDto);
            if (!result.Success) return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetBook), new { id = result.Data?.Id }, result.Data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBook([FromRoute] int id, [FromBody] UpdateBookDto updateBookDto)
        {
            var result = await _bookService.UpdateBookAsync(id, updateBookDto);
            if (!result.Success)
                return result.ErrorCode switch
                {
                    404 => NotFound(result.ErrorMessage),
                    _ => StatusCode(500, result.ErrorMessage)
                };

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBook([FromRoute] int id)
        {
            var result = await _bookService.DeleteBookAsync(id);
            if (!result.Success)
                return result.ErrorCode switch
                {
                    400 => BadRequest(result.ErrorMessage),
                    404 => NotFound(result.ErrorMessage),
                    _ => StatusCode(500, result.ErrorMessage)
                };

            return NoContent();
        }
    }
}
