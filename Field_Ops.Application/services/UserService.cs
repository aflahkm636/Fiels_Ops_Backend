using BCrypt.Net;
using Field_Ops.Application.common;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.UserDto;

namespace Field_Ops.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _repo;
        private readonly IEmailService _emailService;

        public UserService(IUsersRepository repo, IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }


        public async Task<ApiResponse<dynamic?>> GetUserByEmailAsync(string email)
        {
            var user = await _repo.GetUserByEmailAsync(email);

            return user != null
                ? ApiResponse<dynamic?>.SuccessResponse(user)
                : ApiResponse<dynamic?>.FailResponse("User not found", 404);
        }


        public async Task<ApiResponse<IEnumerable<dynamic>>> GetAllAsync()
        {
            var list = await _repo.GetAllAsync();
            return ApiResponse<IEnumerable<dynamic>>.SuccessResponse(list);
        }



        public async Task<ApiResponse<dynamic?>> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);

            return user != null
                ? ApiResponse<dynamic?>.SuccessResponse(user)
                : ApiResponse<dynamic?>.FailResponse("User not found", 404);
        }


        
        public async Task<ApiResponse<bool>> UpdateUserAsync(UserUpdateDto dto)
        {
            var existingUser = await _repo.GetByIdAsync(dto.Id);
            if (existingUser == null)
                return ApiResponse<bool>.FailResponse("User not found", 404);

            var ok = await _repo.UpdateUserAsync(dto);

            return ok
                ? ApiResponse<bool>.SuccessResponse(true, "User updated successfully")
                : ApiResponse<bool>.FailResponse("Failed to update user", 400);
        }


        
        public async Task<ApiResponse<bool>> DeleteUserAsync(int id, int deletedBy)
        {
            var existing = await _repo.GetByIdAsync(id);

            if (existing == null)
                return ApiResponse<bool>.FailResponse("User not found", 404);

            var ok = await _repo.DeleteUserAsync(id, deletedBy);

            return ok
                ? ApiResponse<bool>.SuccessResponse(true, "User deleted successfully")
                : ApiResponse<bool>.FailResponse("Failed to delete user", 400);
        }


        public async Task<ApiResponse<string?>> GetPasswordHashAsync(int userId)
        {
            var hash = await _repo.GetPasswordHashAsync(userId);

            return hash != null
                ? ApiResponse<string?>.SuccessResponse(hash)
                : ApiResponse<string?>.FailResponse("User not found", 404);
        }

        public async Task<ApiResponse<string>> ChangePasswordAsync(ChangePasswordDto dto)
        {
            var currentHash = await _repo.GetPasswordHashAsync(dto.UserId);

            if (currentHash == null)
                return new ApiResponse<string>(404, "User not found.");

            if (!BCrypt.Net.BCrypt.Verify(dto.CurrentPassword, currentHash))
                return new ApiResponse<string>(400, "Current password is incorrect.");

            var newHash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            var changed = await _repo.ChangePasswordAsync(dto.UserId, newHash, dto.ModifiedBy);
            if (!changed)
                return new ApiResponse<string>(400, "Failed to change password.");

            return new ApiResponse<string>(200, "Password changed successfully.");
        }


        
        public async Task<ApiResponse<string>> SendOtpAsync(ForgotPasswordDto dto)
        {
            var user = await _repo.GetUserByEmailAsync(dto.Email);

            if (user == null)
                return new ApiResponse<string>(404, "No user found with this email.");

            string otp = new Random().Next(100000, 999999).ToString();
            DateTime expiry = DateTime.UtcNow.AddMinutes(10);

            var saved = await _repo.SaveResetOtpAsync(dto.Email, otp, expiry);
            if (!saved)
                return new ApiResponse<string>(400, "Failed to generate OTP.");
            Console.WriteLine(saved);

            string body = $@"
                <h2>Password Reset</h2>
                <p>Your OTP code is:</p>
                <h1 style='color:#2d89ef'>{otp}</h1>
                <p>Valid for 10 minutes.</p>
            ";

            await _emailService.SendEmailAsync(dto.Email, "FieldOps Password Reset OTP", body);

            return new ApiResponse<string>(200, "OTP sent to email.");
        }


        public async Task<ApiResponse<string>> ResetPasswordAsync(ResetPasswordDto dto)
        {
            var (otp, expiry) = await _repo.GetResetOtpAsync(dto.Email);

            if (otp == null)
                return new ApiResponse<string>(404, "OTP not found.");

            if (expiry < DateTime.UtcNow)
                return new ApiResponse<string>(400, "OTP has expired.");

            if (otp != dto.Otp)
                return new ApiResponse<string>(400, "Incorrect OTP.");

            string hash = BCrypt.Net.BCrypt.HashPassword(dto.NewPassword);

            var ok = await _repo.ResetPasswordAsync(dto.Email, hash);
            if (!ok)
                return new ApiResponse<string>(400, "Failed to reset password.");

            return new ApiResponse<string>(200, "Password reset successful.");
        }
    }
}
