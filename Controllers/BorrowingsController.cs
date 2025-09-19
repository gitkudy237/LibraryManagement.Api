using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
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
        private readonly IBorrowingService _borrowingService;

        public BorrowingsController(IUnitOfWork unitOfWork, IBorrowingService borrowingService)
        {
            _unitOfWork = unitOfWork;
            _borrowingService = borrowingService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowingWithStatusDto>>> GetAllBorrowings()
        {
            var result = await _borrowingService.GetAllBorrowingsAsync();
            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowingWithStatusDto>> GetBorrowing(int id)
        {
            var result = await _borrowingService.GetBorrowingByIdAsync(id);

            return result is null ?
                NotFound() : Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<BorrowingDto>>> GetOverdueBorrowings()
        {
            var result = await _borrowingService.GetOverdueBorrowingsAsync();

            return Ok(result);
        }

        [Authorize(Roles = "Member")]
        [HttpGet("member")]
        public async Task<ActionResult<IEnumerable<BorrowingWithStatusDto>>> GetMemberBorrowings()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Forbid();

            var result = await _borrowingService.GetMemberBorrowingsAsync(userId);

            return Ok(result);
        }

        [Authorize(Roles = "Member")]
        [HttpPost]

        public async Task<ActionResult> CreateBorrowing([FromBody] CreateBorrowingDto createBorrowingDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();

            var result = await _borrowingService.BorrowBookAsync(userId, createBorrowingDto);
            if (!result.Success)
                return result.ErrorCode switch
                {
                    400 => BadRequest(result.ErrorMessage),
                    404 => NotFound(result.ErrorMessage),
                    _ => StatusCode(result.ErrorCode ?? 500, result.ErrorMessage)
                };

            return CreatedAtAction(
                    nameof(GetBorrowing), new { id = result.Data?.Id },
                    result.Data);

        }

        [Authorize(Roles = "Member")]
        [HttpPut("{bookId:int}/return")]
        public async Task<ActionResult> ReturnBook([FromRoute] int bookId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Forbid();
            var result = await _borrowingService.ReturnBookAsync(userId, bookId);

            if (!result.Success)
                return result.ErrorCode switch
                {
                    400 => BadRequest(result.ErrorMessage),
                    404 => NotFound(result.ErrorMessage),
                    _ => StatusCode(result.ErrorCode ?? 500, result.ErrorMessage)
                };
            return NoContent();
        }
    }
}