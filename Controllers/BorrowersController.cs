using LibraryManagement.Dtos.BorrowerDtos;
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
    public class BorrowersController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowersController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<BorrowerDto>> CreateBorrower([FromBody] CreateBorrowerDto createBorrowerDto)
        {
            var borrower = createBorrowerDto.ToBorrowerModel();

            await _context.Borrowers.AddAsync(borrower);
            await _context.SaveChangesAsync();

            return Ok(borrower.ToBorrowerDto());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowerDto>> GetBorrower([FromRoute] int id)
        {
            var borrower = await _context.Borrowers.FindAsync(id);

            return borrower is null ?
                NotFound() : Ok(borrower.ToBorrowerDto());
        }

        [HttpGet]
        public async Task<ActionResult<List<BorrowerDto>>> GetBorrowers()
        {
            var borrowers = await _context.Borrowers.ToListAsync();
            var result = borrowers.Select(b => b.ToBorrowerDto());

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBorrower([FromRoute] int id, [FromBody] UpdateBorrowerDto updateBorrowerDto)
        {
            var existingBorrower = await _context.Borrowers.FindAsync(id);

            if (existingBorrower is null)
                return NotFound();

            existingBorrower.MapUpdateBorrower(updateBorrowerDto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBorrower([FromRoute] int id)
        {
            var existingBorrower = await _context.Borrowers.FindAsync(id);

            if (existingBorrower is null)
                return NotFound();

            _context.Borrowers.Remove(existingBorrower);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
