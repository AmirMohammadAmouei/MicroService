using Mango.Services.AuthAPI.Models.Dto;

namespace Mango.Services.AuthAPI.Services.Contracts
{
    public interface IAuthService
    {
        Task<string> Register(RegistrationRequestDto registerRequestDto);
        Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
    }
}
