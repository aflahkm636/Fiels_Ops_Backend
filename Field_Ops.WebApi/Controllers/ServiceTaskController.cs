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
    private readonly IServiceTasksService _service;
    private readonly IEmployeesRepository _empRepo;

    public ServiceTasksController(IServiceTasksService service, IEmployeesRepository employeesRepo)
    {
        _service = service;
        _empRepo = employeesRepo;
    }

    [HttpPost]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Create([FromBody] ServiceTaskCreateDto dto)
    {
        try
        {
            dto.ActionUserId = User.GetUserId();

            var response = await _service.CreateAsync(dto);
            return StatusCode(response.StatusCode, response);
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
            var response = await _service.GetAllAsync();
            return StatusCode(response.StatusCode, response);
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
            var response = await _service.GetByIdAsync(id);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Update([FromBody] ServiceTaskUpdateDto dto)
    {
        try
        {
            //dto.ActionUserId = User.GetUserId();

            dto.ActionUserId = 3;
            var response = await _service.UpdateAsync(dto);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Staff,Technician,Admin")]
    public async Task<IActionResult> UpdateStatus([FromBody] ServiceTaskUpdateStatusDto dto)
    {
        try
        {

            //dto.ActionUserId = User.GetUserId();
            dto.ActionUserId = 3;

            var response = await _service.UpdateStatusAsync(dto);
            return StatusCode(response.StatusCode, response);
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

            var response = await _service.DeleteAsync(id, actionUserId);
            return StatusCode(response.StatusCode, response);
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

            var response = await _service.GetTasksByCustomerAsync(userId);
            return StatusCode(response.StatusCode, response);
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
            var response = await _service.GetTasksByStatusAsync(status);
            return StatusCode(response.StatusCode, response);
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
                if (resolvedEmployeeId <= 0)
                    return StatusCode(400, new { Message = "EmployeeId could not be resolved for this user." });

                employeeId = resolvedEmployeeId;
            }

            var response = await _service.GetTasksByTechnicianAsync(employeeId.Value);
            return StatusCode(response.StatusCode, response);
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
            var response = await _service.GetTasksBySubscriptionIdAsync(subscriptionId);
            return StatusCode(response.StatusCode, response);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}
