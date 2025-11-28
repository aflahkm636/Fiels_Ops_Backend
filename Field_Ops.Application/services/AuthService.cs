using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.CustomerDto;
using Field_Ops.Application.DTO.UserDto;
using Field_Ops.Application.DTOs.AuthDto;

namespace Field_Ops.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public AuthService(ICustomerRepository customerRepository , IUsersRepository usersRepository ,IJwtTokenService jwtTokenService )
        {
            _customerRepository = customerRepository;
            _usersRepository = usersRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<AuthResponseDto> RegisterUserAsync(CustomerRegisterDto dto)
        {
            dto.Email = dto.Email.Trim().ToLower();
            dto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

             await _customerRepository.AddCustomerAsync(dto);

            return new AuthResponseDto(
                statusCode: 201,
                message: "User + Customer registered successfully"

            );
        }

        public async Task<AuthResponseDto> LoginAsync(UserLoginDto dto)
        {
            var user = await _usersRepository.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return new AuthResponseDto(400, "Invalid credentials");
            //if (!user.Active)
            //    return new AuthResponseDto(403, "Account is inactive");
            if (user.IsDeleted)
                return new AuthResponseDto(403, "Account has been deleted");


            bool valid = BCrypt.Net.BCrypt.Verify(dto.Password, (string)user.PasswordHash);
            if (!valid)
                return new AuthResponseDto(400, "Invalid credentials");

            var jwtUser = UserDtoMapper.FromDynamic(user);

            string token = _jwtTokenService.GenerateToken(jwtUser);

            return new AuthResponseDto(200, "Login successful", token);
        }

    }
}
