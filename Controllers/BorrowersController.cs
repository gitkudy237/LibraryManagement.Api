using LibraryManagement.Dtos.BorrowerDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Models;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<Borrower>> CreateBorrower([FromBody] CreateBorrowerDto createBorrowerDto)
        {
            var borrower = createBorrowerDto.ToBorrowerModel();

            await _context.Borrowers.AddAsync(borrower);
            await _context.SaveChangesAsync();

            return Ok(borrower.ToBorrowerDto());
        }
    }
}
