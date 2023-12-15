using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.AuthAPI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _db;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthService(AppDbContext db, IJwtTokenGenerator jwtTokenGenerator, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _jwtTokenGenerator = jwtTokenGenerator;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> Register(RegistrationRequestDto registerRequestDto)
        {
            ApplicationUser user = new()
            {
                UserName = registerRequestDto.Email,
                Email = registerRequestDto.Email,
                NormalizedEmail = registerRequestDto.Email.ToUpper(),
                Name = registerRequestDto.Name,
                PhoneNumber = registerRequestDto.PhoneNumber,
            };

            try
            {
                var result = await _userManager.CreateAsync(user, registerRequestDto.Password);

                if (result.Succeeded)
                {
                    //we don't want to return anything to client when user was created so we return empty string
                    var userToReturn = _db.Users.First(x => x.UserName == registerRequestDto.Email);


                    UserDto userDto = new()
                    {
                        Email = userToReturn.Email,
                        Id = userToReturn.Id,
                        Name = userToReturn.Name,
                        PhoneNumber = userToReturn.PhoneNumber
                    };

                    return "";
                }
                else
                {
                    return result.Errors.FirstOrDefault().Description;
                }

            }
            catch (Exception e)
            {

            }

            return "Error Encountered";
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var findUserById =
                await _db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginRequestDto.UserName.ToLower());

            bool isValid = await _userManager.CheckPasswordAsync(findUserById, loginRequestDto.Password);

            if (findUserById == null || isValid == false)
            {
                return new LoginResponseDto() { User = null, Token = "" };
            }

            //if user was found generate token
            var token = _jwtTokenGenerator.GenerateToken(findUserById);

            UserDto userDto = new()
            {
                Email = findUserById.Email,
                Id = findUserById.Id,
                Name = findUserById.Name,
                PhoneNumber = findUserById.PhoneNumber
            };

            //init User obj through the UserDto
            LoginResponseDto loginResponseDto = new()
            {
                User = userDto,
                Token = token
            };

            return loginResponseDto;
        }
    }
}
