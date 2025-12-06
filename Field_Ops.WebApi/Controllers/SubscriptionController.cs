using Field_ops.Domain.Enums;
using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO;
using Field_Ops.Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ServiceTasksController : ControllerBase
{
    private readonly IServiceTasksService _serviceTasksService;
    private readonly IEmployeesRepository _empRepo;

    public ServiceTasksController(IServiceTasksService service, IEmployeesRepository employeesRepo)
    {
        _service=service;
        _empRepo = employeesRepo;
    }


    [HttpPost]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Create([FromBody] ServiceTaskCreateDto dto)
    {
        try
        {
            dto.ActionUserId = User.GetUserId();

            var result = await _service.CreateAsync(dto);
            return StatusCode(200, new { TaskId = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _service.GetAllAsync();
            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

 
    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Staff,Technician")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);

            if (result == null)
                return StatusCode(404, new { Message = "Task not found" });

            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpPut("{id:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceTaskUpdateDto dto)
    {
        try
        {
            dto.Id = id;
            dto.ActionUserId = User.GetUserId();

            var rows = await _service.UpdateAsync(dto);

            if (rows > 0)
                return StatusCode(200, new { Updated = true });

            return StatusCode(400, new { Message = "Update failed" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Staff,Technician,Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] ServiceTaskUpdateStatusDto dto)
    {
        try
        {
            dto.Id = id;
            dto.ActionUserId = User.GetUserId();

            var rows = await _service.UpdateStatusAsync(dto);

            if (rows > 0)
                return StatusCode(200, new { Updated = true });

            return StatusCode(400, new { Message = "Status update failed" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            int actionUserId = User.GetUserId();

            var rows = await _service.DeleteAsync(id, actionUserId);

            if (rows > 0)
                return StatusCode(200, new { Deleted = true });

            return StatusCode(400, new { Message = "Delete failed" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpGet("customer")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetTasksForCustomer()
    {
        try
        {
            int userId = User.GetUserId();

            var result = await _service.GetTasksByCustomerAsync(userId);
            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetTasksByStatus(ServiceTaskStatus status)
    {
        try
        {
            var result = await _service.GetTasksByStatusAsync(status);
            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpGet("technician")]
    [Authorize(Roles = "Technician,Admin,Staff")]
    public async Task<IActionResult> GetTasksByTechnician([FromQuery] int? employeeId)
    {
        try
        {
            if (employeeId == null)
            {
                int userId = User.GetUserId();

                int resolvedEmployeeId = await _empRepo.GetEmployeeIdByUSerID(userId);
                if (resolvedEmployeeId<0)
                    return StatusCode(400, new { Message = "EmployeeId could not be resolved for this user." });

                employeeId = resolvedEmployeeId;
            }

            var result = await _service.GetTasksByTechnicianAsync(employeeId.Value);
            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }


    [HttpGet("subscription/{subscriptionId:int}")]
    [Authorize(Roles = "Admin,Staff,Technician")]
    public async Task<IActionResult> GetTasksBySubscription(int subscriptionId)
    {
        try
        {
            var result = await _service.GetTasksBySubscriptionIdAsync(subscriptionId);
            return StatusCode(200, result);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}
