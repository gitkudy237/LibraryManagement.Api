using LibraryManagement.Core.Abstractions;
using LibraryManagement.Dtos.BorrowerDtos;
using LibraryManagement.Mappings;
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
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BorrowersController(IBorrowerRepository borrowerRepository,
         IUnitOfWork unitOfWork)
        {
            _borrowerRepository = borrowerRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<BorrowerDto>> CreateBorrower([FromBody] CreateBorrowerDto createBorrowerDto)
        {
            var borrower = createBorrowerDto.ToBorrowerModel();

            await _borrowerRepository.AddAsync(borrower);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetBorrower), new { id = borrower.Id }, borrower.ToBorrowerDto());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<BorrowerDto>> GetBorrower([FromRoute] int id)
        {
            var borrower = await _borrowerRepository.GetByIdAsync(id);

            return borrower is null ?
                NotFound() : Ok(borrower.ToBorrowerDto());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BorrowerDto>>> GetBorrowers()
        {
            var borrowers = await _borrowerRepository.GetAllAsync();
            var result = borrowers.Select(b => b.ToBorrowerDto());

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateBorrower([FromRoute] int id, [FromBody] UpdateBorrowerDto updateBorrowerDto)
        {
            var existingBorrower = await _borrowerRepository.GetByIdAsync(id);

            if (existingBorrower is null)
                return NotFound();

            existingBorrower.MapUpdateBorrower(updateBorrowerDto);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteBorrower([FromRoute] int id)
        {
            var existingBorrower = await _borrowerRepository.GetByIdAsync(id);

            if (existingBorrower is null)
                return NotFound();

            _borrowerRepository.Delete(existingBorrower);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
