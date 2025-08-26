using LibraryManagement.Dtos.AuthorDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Models;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AuthorsController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public AuthorsController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Author>> GetAuthor([FromRoute] int id)
    {
        var author = await _context.Authors.FindAsync(id);

        return author is null ?
            NotFound() : Ok(author);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        var author = createAuthorDto.ToAuthorModel();
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        return Ok(author);
    }
}
