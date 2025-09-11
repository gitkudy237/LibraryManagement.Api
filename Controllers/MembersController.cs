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
    public class MembersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public MembersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetMember([FromRoute] int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);

            return member is null ?
                NotFound() : Ok(member.ToMemberDto());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            var result = members.Select(b => b.ToMemberDto());

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMember([FromRoute] int id, [FromBody] UpdateMemberDto updateMemberDto)
        {
            var existingMember = await _unitOfWork.Members.GetByIdAsync(id);

            if (existingMember is null)
                return NotFound();

            existingMember.MapUpdateMember(updateMemberDto);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMember([FromRoute] int id)
        {
            var existingMember = await _unitOfWork.Members.GetByIdAsync(id);

            if (existingMember is null)
                return NotFound();

            _unitOfWork.Members.Delete(existingMember);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
