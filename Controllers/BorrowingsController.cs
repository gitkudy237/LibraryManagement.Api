using LibraryManagement.Core.Abstractions;
using LibraryManagement.Dtos.BorrowingsDto;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetAllBorrowings()
        {
            var borrowings = await _unitOfWork.Borrowings.GetAllAsync();
            var result = borrowings.Select(b => b.ToBorrowingWithStatusDto());

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingDto>> GetBorrowing(int id)
        {
            var borrowing = await _unitOfWork.Borrowings.GetByIdAsync(id, includeRelated: true);

            return borrowing is null ?
                NotFound() : Ok(borrowing.ToBorrowingDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetOverdueBorrowings()
        {
            var overdueBorrowings = await _unitOfWork.Borrowings.GetOverdue();
            var result = overdueBorrowings.Select(ob => ob.ToBorrowingDto());

            return Ok(result);
        }

        [Authorize(Roles = "Member")]
        [HttpGet("member")]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetMemberBorrowings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Forbid();

            var borrowings = await _unitOfWork.Borrowings.GetMemberBorrowings(userId);
            var result = borrowings.Select(b => b.ToBorrowingDto());

            return Ok(result);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]
        public async Task<ActionResult> BorrowBook([FromBody] CreateBorrowingDto createBorrowingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            var member = await _unitOfWork.Members.GetByUserIdAsync(userId);
            if (member is null)
                return BadRequest("Borrower not found");

            var borrowingRequest = createBorrowingDto.ToBorrowingModel(member.Id);
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

        [Authorize(Roles = "Member")]
        [HttpPut("return/{borrowingId:int}")]
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