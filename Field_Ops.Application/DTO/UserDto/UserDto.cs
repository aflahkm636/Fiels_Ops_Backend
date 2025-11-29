using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
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

    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Role { get; set; }
        public bool? Status { get; set; }
        public string? ProfileImage { get; set; }

        [JsonIgnore]
        public int ModifiedBy { get; set; }
    }

    public class ForgotPasswordDto
    {
        public string Email { get; set; } = string.Empty;
    }

 
        public class ChangePasswordDto
        {
        [JsonIgnore]
        public int UserId { get; set; }

            public string CurrentPassword { get; set; } = string.Empty;
            public string NewPassword { get; set; } = string.Empty;
            public int ModifiedBy { get; set; }
        }

    public class ResetPasswordDto
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
    }

    public class LoggedInUser
    {
        public int UserId { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}
