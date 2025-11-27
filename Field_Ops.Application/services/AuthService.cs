using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.CustomerDto;
using Field_Ops.Application.DTOs.AuthDto;

namespace Field_Ops.Application.Services
{
    public class AuthService:IAuthService
    {
        private readonly ICustomerRepository _customerRepository;

        public AuthService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<AuthReponseDto> RegisterUserAsync(CustomerRegisterDto dto)
        {
            dto.Email = dto.Email.Trim().ToLower();
            dto.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

             await _customerRepository.AddCustomerAsync(dto);

            return new AuthReponseDto(
                statusCode: 201,
                message: "User + Customer registered successfully"

            );
        }
    }
}
