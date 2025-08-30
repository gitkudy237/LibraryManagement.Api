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
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MembersController(IMemberRepository memberRepository,
         IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<MemberDto>> CreateMember([FromBody] CreateMemberDto CreateMemberDto)
        {
            var member = CreateMemberDto.ToMemberModel();

            await _memberRepository.AddAsync(member);
            await _unitOfWork.CommitAsync();

            return CreatedAtAction(nameof(GetMember), new { id = member.Id }, member.ToMemberDto());
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<MemberDto>> GetMember([FromRoute] int id)
        {
            var member = await _memberRepository.GetByIdAsync(id);

            return member is null ?
                NotFound() : Ok(member.ToMemberDto());
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetAllMembers()
        {
            var members = await _memberRepository.GetAllAsync();
            var result = members.Select(b => b.ToMemberDto());

            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> UpdateMember([FromRoute] int id, [FromBody] UpdateMemberDto updateMemberDto)
        {
            var existingMember = await _memberRepository.GetByIdAsync(id);

            if (existingMember is null)
                return NotFound();

            existingMember.MapUpdateMember(updateMemberDto);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> DeleteMember([FromRoute] int id)
        {
            var existingMember = await _memberRepository.GetByIdAsync(id);

            if (existingMember is null)
                return NotFound();

            _memberRepository.Delete(existingMember);
            await _unitOfWork.CommitAsync();

            return NoContent();
        }
    }
}
