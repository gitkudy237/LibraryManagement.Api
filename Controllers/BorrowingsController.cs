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
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IBorrowingRepository _borrowingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingsController(IBorrowingRepository borrowingRepository, IUnitOfWork unitOfWork,
         IBookRepository bookRepository, IBorrowerRepository borrowerRepository)
        {
            _bookRepository = bookRepository;
            _borrowingRepository = borrowingRepository;
            _unitOfWork = unitOfWork;
            _borrowerRepository = borrowerRepository;
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

        [HttpPost]
        public async Task<ActionResult> AddBorrowing([FromBody] CreateBorrowingDto createBorrowingDto)
        {
            if (createBorrowingDto.DueDate < createBorrowingDto.BorrowingDate)
                return BadRequest("Due date is sooner than borrowing date");

            var borrowingRequest = createBorrowingDto.ToBorrowingModel();

            var borrower = await _borrowerRepository.GetByIdAsync(borrowingRequest.BorrowerId);
            if (borrower is null)
                return BadRequest("Borrower not found");

            var book = await _bookRepository.GetByIdAsync(borrowingRequest.BookId);
            if (book is null)
                return BadRequest("Book not found");
            if (book.AvailableCopies < 1)
                return BadRequest("Book is not availble at the moment");

            book.AvailableCopies--;

            await _borrowingRepository.AddAsync(borrowingRequest);
            await _unitOfWork.CommitAsync();

            return Ok(borrowingRequest.ToBorrowingDto());

        }
    }
}