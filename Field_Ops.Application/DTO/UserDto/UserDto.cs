using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Field_Ops.Application.DTO.UserDto
{
    public class UserRegisterDto
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? Phone { get; set; }
        public string PasswordHash { get; set; } = default!;
        public string Role { get; set; } = default!;
        public int CreatedBy { get; set; }
    }

    public class UserForJwtDto
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Role { get; set; } = default!;
    }

    public static class UserDtoMapper
    {
        public static UserForJwtDto FromDynamic(dynamic u)
        {
            if (u == null)
                throw new ArgumentNullException(nameof(u));

            return new UserForJwtDto
            {
                UserId = u.Id,           
                UserEmail = u.Email,
                UserName = u.Name,
                Role = u.Role
            };
        }
    }

    public class UserLoginDto
    {
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }


}
