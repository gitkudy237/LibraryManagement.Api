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
        private readonly IUnitOfWork _unitOfWork;

        public BorrowingsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetAllBorrowings()
        {
            var borrowings = await _unitOfWork.Borrowings.GetAllAsync();
            var result = borrowings.Select(b => b.ToBorrowingWithStatusDto());

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingDto>> GetBorrowing(int id)
        {
            var borrowing = await _unitOfWork.Borrowings.GetByIdAsync(id, includeRelated: true);

            return borrowing is null ?
                NotFound() : Ok(borrowing.ToBorrowingDto());
        }

        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetOverdueBorrowings()
        {
            var overdueBorrowings = await _unitOfWork.Borrowings.GetOverdue();
            var result = overdueBorrowings.Select(ob => ob.ToBorrowingDto());

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> BorrowBook([FromBody] CreateBorrowingDto createBorrowingDto)
        {
            if (createBorrowingDto.DueDate < createBorrowingDto.BorrowingDate)
                return BadRequest("Due date is sooner than borrowing date");

            var borrowingRequest = createBorrowingDto.ToBorrowingModel();

            var member = await _unitOfWork.Members.GetByIdAsync(borrowingRequest.MemberId);
            if (member is null)
                return BadRequest("Borrower not found");

            var book = await _unitOfWork.Books.GetByIdAsync(borrowingRequest.BookId);
            if (book is null)
                return BadRequest("Book not found");
            if (book.AvailableCopies < 1)
                return BadRequest("Book is not availble at the moment");

            book.AvailableCopies--;

            await _unitOfWork.Borrowings.AddAsync(borrowingRequest);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(
                nameof(GetBorrowing), new { id = borrowingRequest.Id },
                borrowingRequest.ToBorrowingDto());

        }

        [HttpPut("{borrowingId:int}")]
        public async Task<ActionResult> ReturnBook([FromRoute] int borrowingId)
        {
            var activeBorrowing = await _unitOfWork.Borrowings
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