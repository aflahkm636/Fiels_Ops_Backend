using Field_Ops.Application.DTO.UserDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.Contracts.Repository
{
    public interface IUsersRepository
    {
        Task<int> RegisterUserAsync(UserRegisterDto dto);
        Task<dynamic?> GetUserByEmailAsync(string email);
        Task<dynamic?> GetByIdAsync(int id);
        Task<IEnumerable<dynamic>> GetAllAsync();
    }
}
