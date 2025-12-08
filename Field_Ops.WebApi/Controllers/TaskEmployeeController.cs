using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.TaskEMployeeDto;
using Field_Ops.Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaskEmployeesController : ControllerBase
{
    private readonly ITaskEmployeesService _service;
    private readonly IEmployeesRepository _empRepo;

    public TaskEmployeesController(ITaskEmployeesService service, IEmployeesRepository empRepo)
    {
        _service = service;
        _empRepo = empRepo;
    }

    [HttpPost("assign")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Assign([FromBody] TaskEmployeeAssignDto dto)
    {
        try
        {
            dto.ActionUserId = User.GetUserId();

            var response = await _service.AssignAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:int}/remove")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Remove(int id)
    {
        try
        {
            int actionUserId = User.GetUserId();

            var response = await _service.RemoveAsync(id, actionUserId);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("task/{taskId:int}")]
    [Authorize(Roles = "Admin,Staff,Technician")]
    public async Task<IActionResult> GetByTask(int taskId)
    {
        try
        {
            var response = await _service.GetByTaskAsync(taskId);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("employee/{employeeId:int}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetByEmployee(int employeeId)
    {
        try
        {
            var response = await _service.GetByEmployeeAsync(employeeId);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("technician")]
    [Authorize(Roles = "Technician,Admin,Staff")]
    public async Task<IActionResult> GetTasksByTechnicianStatus([FromQuery] string? status, [FromQuery] int? employeeId)
    {
        try
        {
            if (employeeId == null)
            {
                int userId = User.GetUserId();
                int resolvedEmployeeId = await _empRepo.GetEmployeeIdByUSerID(userId);

                if (resolvedEmployeeId <= 0)
                    return StatusCode(400, new { Message = "EmployeeId could not be resolved for this user." });

                employeeId = resolvedEmployeeId;
            }

            var response = await _service.GetTasksByTechnicianStatusAsync(employeeId.Value, status);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPut("update")]
    [Authorize(Roles = "Admin,Staff")]

    public async Task<IActionResult> Update(TaskEmployeeUpdateDto dto)
    {
        dto.ActionUserId=User.GetUserId();
        var response = await _service.UpdateAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

}
