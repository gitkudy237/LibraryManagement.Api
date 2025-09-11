using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LibraryManagement.Core.Abstractions;
using LibraryManagement.Core.Models;
using LibraryManagement.Dtos.AppUserDtos;
using LibraryManagement.Mappings;
using LibraryManagement.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork, IAuthService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        private static ApplicationUser _user = new();

        [HttpPost("register/member")]
        public async Task<IActionResult> RegisterMember(MemberRegistrationDto memberRegistration)
        {
            var registrationRequest = memberRegistration.ToRegistrationRequest();
            var appUser = await _authService.RegisterAsync(registrationRequest);

            if (appUser is null)
                return BadRequest("User already exists.");

            var member = memberRegistration.ToMemberModel(appUser.Id);
            await _unitOfWork.Members.AddAsync(member);

            await _unitOfWork.CommitAsync();

            return Ok(appUser.ToAppUserDto());
        }

        [HttpPost("register/admin")]
        public async Task<IActionResult> RegisterAdmin(AdminRegistrationDto adminRegistration)
        {
            var registrationRequest = adminRegistration.ToRegistrationRequest();
            var appUser = await _authService.RegisterAsync(registrationRequest);

            if (appUser is null)
                return BadRequest("User already exists.");

            await _unitOfWork.CommitAsync();

            return Ok(appUser.ToAppUserDto());
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto loginRequest)
        {
            var response = await _authService.LoginAsync(loginRequest);

            return response is null ?
                BadRequest("Incorrect username or password") : Ok(response);
        }
    }


}