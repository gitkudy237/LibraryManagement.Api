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
            var result = borrowings.Select(b => b.ToBorrowingWithStatusDto());

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingDto>> GetBorrowing(int id)
        {
            var borrowing = await _borrowingRepository.GetByIdAsync(id, includeRelated: true);

            return borrowing is null ?
                NotFound() : Ok(borrowing.ToBorrowingDto());
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetOverdueBorrowings()
        {
            var overdueBorrowings = await _borrowingRepository.GetOverdue();
            var result = overdueBorrowings.Select(ob => ob.ToBorrowingDto());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> BorrowBook([FromBody] CreateBorrowingDto createBorrowingDto)
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

            return CreatedAtAction(
                nameof(GetBorrowing), new { id = borrowingRequest.Id },
                borrowingRequest.ToBorrowingDto());

        }

        [HttpPut("{borrowingId:int}")]
        public async Task<ActionResult> ReturnBook([FromRoute] int borrowingId)
        {
            var activeBorrowing = await _borrowingRepository
                .GetByIdAsync(borrowingId, includeRelated: true);

            if (activeBorrowing is null)
                return BadRequest("Borrowing not found");
            if (activeBorrowing.ReturnDate is not null)
                return BadRequest("Book already returned");

            activeBorrowing.ReturnDate = DateOnly.FromDateTime(DateTime.Now);
            activeBorrowing.Book.AvailableCopies++;

            await _unitOfWork.CommitAsync();

            return NoContent();

        }
    }
}