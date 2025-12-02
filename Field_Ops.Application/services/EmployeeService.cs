using Field_Ops.Application.common;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.EmployeeDto;
using Field_Ops.Application.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Field_Ops.Application.Service
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _repo;
        private readonly IEmailService _emailService;

        public EmployeesService(IEmployeesRepository repo ,IEmailService emailService)
        {
            _repo = repo;
            _emailService = emailService;
        }

     
        public async Task<ApiResponse<bool>> CreateEmployeeAsync(EmployeeCreateDto dto)

        {
            //if (string.IsNullOrWhiteSpace(dto.Name))
            //    throw new ArgumentException("Name is required.");

            //if (string.IsNullOrWhiteSpace(dto.Email))
            //    throw new ArgumentException("Email is required.");

            //if (string.IsNullOrWhiteSpace(dto.PasswordHash))
            //    throw new ArgumentException("PasswordHash is required.");


            //if (dto.Role.ToString() == "Technician" && dto.DepartmentId != 2)

            string tempPassword = string.IsNullOrWhiteSpace(dto.Password)
              ? PasswordHelper.GenerateTemporaryPassword()
              : dto.Password;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(tempPassword);

            dto.Password = hashedPassword;

            var isCreated = await _repo.CreateEmployeeAsync(dto);

            if (!isCreated)
                return ApiResponse<bool>.FailResponse(400,"Employee creation failed");

            string subject = "Welcome to Field_Ops";
            string body = $@"
        <h3>Hello {dto.Name},</h3>
        <p>Your Field_Ops account has been created successfully.</p>
        <p><b>Email:</b> {dto.Email}<br>
        <b>Temporary Password:</b> {tempPassword}</p>
        <p>Please log in and change your password immediately.</p>
        <p>Best regards,<br>Field_Ops Team</p>";

            await _emailService.SendEmailAsync(dto.Email,subject,body);

            return ApiResponse<bool>.SuccessResponse(201, $"Staff created successfully! Credentials sent to {dto.Email}.");
        }

        public async Task<ApiResponse<IEnumerable<dynamic>>> GetAllAsync()
        {
            var Employees = await _repo.GetAllAsync();

            return ApiResponse<IEnumerable<dynamic>>
                .SuccessResponse(200, "Employees fetched successfully", Employees);
        }

     
        public async Task<ApiResponse<dynamic?>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return ApiResponse<dynamic?>
                    .FailResponse(400,"Invalid Employee ID");

            var emp = await _repo.GetByIdAsync(id);

            return emp != null
                ? ApiResponse<dynamic?>.SuccessResponse(200, "Employee fetched successfully", emp)
                : ApiResponse<dynamic?>.FailResponse(404,"Employee not found");
        }

        public async Task<ApiResponse<bool>> UpdateEmployeeAsync(EmployeeUpdateDto dto)
        {
            if (dto.Id <= 0)
                return ApiResponse<bool>.FailResponse(400,"Invalid Employee ID");

            if (dto.ModifiedBy <= 0)
                return ApiResponse<bool>.FailResponse(400,"ModifiedBy is required");

            var ok = await _repo.UpdateEmployeeAsync(dto);

            return ok
                ? ApiResponse<bool>.SuccessResponse(200, "Employee updated successfully", true)
                : ApiResponse<bool>.FailResponse(400,"Failed to update employee");
        }

  
        public async Task<ApiResponse<bool>> DeleteEmployeeAsync(int id, int deletedBy)
        {
            if (id <= 0)
                return ApiResponse<bool>.FailResponse(400,"Invalid Employee ID");

            if (deletedBy <= 0)
                return ApiResponse<bool>.FailResponse(400,"DeletedBy is required");

            var ok = await _repo.DeleteEmployeeAsync(id, deletedBy);

            return ok
                ? ApiResponse<bool>.SuccessResponse(200, "Employee deleted successfully", true)
                : ApiResponse<bool>.FailResponse(400,"Failed to delete employee");
        }
    }
}
    

