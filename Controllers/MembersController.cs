using System.Security.Claims;
using LibraryManagement.Core.Abstractions;
using LibraryManagement.Dtos.BorrowerDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetMember([FromRoute] int id)
        {
            var member = await _unitOfWork.Members.GetByIdAsync(id);

            return member is null ?
                NotFound() : Ok(member.ToMemberDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            var members = await _unitOfWork.Members.GetAllAsync();
            var result = members.Select(b => b.ToMemberDto());

            return Ok(result);
        }

        [Authorize(Roles = "Member")]
        [HttpPut]
        public async Task<ActionResult> UpdateMember([FromBody] UpdateMemberDto updateMemberDto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null)
                return Forbid();

            var existingMember = await _unitOfWork.Members.GetByUserIdAsync(userId);

            if (existingMember is null)
                return NotFound();

            existingMember.MapUpdateMember(updateMemberDto);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{userId:string}")]
        public async Task<ActionResult> DeleteMember([FromRoute] string userId)
        {
            var existingMember = await _unitOfWork.Members.GetByUserIdAsync(userId);

            if (existingMember is null)
                return NotFound();

            _unitOfWork.Members.Delete(existingMember);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}