using LibraryManagement.Dtos.BorrowingsDto;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowingsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public BorrowingsController(LibraryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetAllBorrowings()
        {
            var borrowings = await _context.Borrowings.ToListAsync();
            var result = borrowings.Select(b => b.ToBorrowingDto());

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingDto>> GetBorrowing(int id)
        {
            var borrowing = await _context.Borrowings.FindAsync(id);

            return borrowing is null ?
                NotFound() : Ok(borrowing.ToBorrowingDto());
        }
    }
}