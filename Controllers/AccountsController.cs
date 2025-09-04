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

// TODO:   Create MemberRegisterDto and AdminRegisterDto
// TODO: Create TokenService
// TODO: Add corresponding settings in appsettings.json
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

        [HttpPost]
        public async Task<IActionResult> Register(MemberRegistrationDto registrationRequest)
        {
            var newUser = await _authService.RegisterAsync(registrationRequest);

            if (newUser is null)
                return BadRequest("User already exists.");

            var newMember = registrationRequest.ToMemberModel(newUser.Id);
            await _unitOfWork.Members.AddAsync(newMember);

            await _unitOfWork.CommitAsync();

            return Ok(_user);
        }
    }


}