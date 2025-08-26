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

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuthor([FromRoute] int id, [FromBody] UpdateAuthorDto updateAuthorDto)
    {
        var existingAuthor = await _context.Authors.FindAsync(id);

        if (existingAuthor is null) 
            return NotFound("Attempt to update unexisting author");

        existingAuthor.MapUpdateAuthor(updateAuthorDto);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuthor([FromRoute] int id)
    {
        var existingAuthor = await _context.Authors
            .Include(auth => auth.Books)
            .FirstOrDefaultAsync(auth => auth.Id == id);

        if (existingAuthor is null)
            return NotFound();

        if (existingAuthor.Books.Count > 0)
            return BadRequest("Cannot delete author with one or more books");

        _context.Authors.Remove(existingAuthor);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
