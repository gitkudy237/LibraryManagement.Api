using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AuthorDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class AuthorsController : ControllerBase
{
    private readonly IAuthorRepository _authorsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AuthorsController(IAuthorRepository authorsRepository, IUnitOfWork unitOfWork)
    {
        _authorsRepository = authorsRepository;
        _unitOfWork = unitOfWork;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AuthorDto>> GetAuthor([FromRoute] int id)
    {
        var author = await _authorsRepository.GetByIdAsync(id);

        return author is null ?
            NotFound() : Ok(author.ToAuthorDto());
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
    {
        var authors = await _authorsRepository.GetAllAsync();
        var result = authors.Select(auth => auth.ToAuthorDto());

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] CreateAuthorDto createAuthorDto)
    {
        var author = createAuthorDto.ToAuthorModel();
        await _authorsRepository.AddAsync(author);
        await _unitOfWork.CommitAsync();

        return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, author.ToAuthorDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateAuthor([FromRoute] int id, [FromBody] UpdateAuthorDto updateAuthorDto)
    {
        var existingAuthor = await _authorsRepository.GetByIdAsync(id);

        if (existingAuthor is null)
            return NotFound("Attempt to update unexisting author");

        existingAuthor.MapUpdateAuthor(updateAuthorDto);
        await _unitOfWork.CommitAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAuthor([FromRoute] int id)
    {
        var existingAuthor = await _authorsRepository.GetByIdAsync(id, includeRelated: true);

        if (existingAuthor is null)
            return NotFound();

        if (existingAuthor.Books.Count > 0)
            return BadRequest("Cannot delete author with one or more books");

        _authorsRepository.Delete(existingAuthor);
        await _unitOfWork.CommitAsync();

        return NoContent();
    }
}
