using LibraryManagement.Core.Abstractions;
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
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingsController(IBorrowingRepository borrowingRepository, IUnitOfWork unitOfWork)
        {
            _borrowingRepository = borrowingRepository;
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetAllBorrowings()
        {
            var borrowings = await _borrowingRepository.GetAllAsync();
            var result = borrowings.Select(b => b.ToBorrowingDto());

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingDto>> GetBorrowing(int id)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(id, includeRelated: true);

            return borrowing is null ?
                NotFound() : Ok(borrowing);
        }
    }
}