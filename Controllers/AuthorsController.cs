using LibraryManagement.Dtos.AuthorDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Models;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<ActionResult<AuthorDto>> GetAuthor([FromRoute] int id)
    {
        var author = await _context.Authors.FindAsync(id);

        return author is null ?
            NotFound() : Ok(author.ToAuthorDto());
    }

    [HttpGet]
    public async Task<ActionResult<List<AuthorDto>>> GetAuthors()
    {
        var authors = await _context.Authors.ToListAsync();
        var result = authors.Select(auth => auth.ToAuthorDto());

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        var author = createAuthorDto.ToAuthorModel();
        await _context.Authors.AddAsync(author);
        await _context.SaveChangesAsync();

        return Ok(author.ToAuthorDto());
    }
}
