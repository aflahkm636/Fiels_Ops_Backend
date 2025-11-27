using Field_Ops.Application.DTO.CustomerDto;
using Field_Ops.Application.DTOs.AuthDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.Contracts.Service
{
    public interface IAuthService
    {
        Task<AuthReponseDto> RegisterUserAsync(CustomerRegisterDto dto);
    }
}
