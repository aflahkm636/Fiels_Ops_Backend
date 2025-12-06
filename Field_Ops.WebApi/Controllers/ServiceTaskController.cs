using Field_ops.Domain.Enums;
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

    public ServiceTasksController(IServiceTasksService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Create(ServiceTaskCreateDto dto)
    {
        dto.ActionUserId = User.GetUserId();
        var id = await _service.CreateAsync(dto);
        return Ok(new { TaskId = id });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(data);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Staff,Technician")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (result == null) return NotFound();

        return Ok(result);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Update(int id, ServiceTaskUpdateDto dto)
    {
        dto.ActionUserId = User.GetUserId();
        

        var rows = await _service.UpdateAsync(dto);

        return rows > 0 ? Ok(new { Updated = true }) : BadRequest("Update failed.");
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Staff,Technician,Admin")]
    public async Task<IActionResult> UpdateStatus( ServiceTaskUpdateStatusDto dto)
    {
        dto.ActionUserId= User.GetUserId();

        var result = await _service.UpdateStatusAsync(dto);

        return result > 0 ? Ok(new { Updated = true }) : BadRequest("Status update failed.");
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Delete(int id)

    {
        int actionUserId= User.GetUserId();
        var rows = await _service.DeleteAsync(id, actionUserId);

        return rows > 0 ? Ok(new { Deleted = true }) : BadRequest("Delete failed.");
    }

    [HttpGet("customer")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> GetTasksForCustomer()
    {
        int actionUserId=User.GetUserId();
        var list = await _service.GetTasksByCustomerAsync(actionUserId);
        return Ok(list);
    }

    [HttpGet("status/{status}")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetTasksByStatus(ServiceTaskStatus status)
    {
        var data = await _service.GetTasksByStatusAsync(status);
        return Ok(data);
    }

    [HttpGet("technician/{employeeId:int}")]
    [Authorize(Roles = "Technician,Admin,Staff")]
    public async Task<IActionResult> GetTasksByTechnician(int? employeeId)
    {
        if (employeeId == null)
            employeeId = User.GetUserId();

        var list = await _service.GetTasksByTechnicianAsync(employeeId);
        return Ok(list);
    }

    [HttpGet("subscription/{subscriptionId:int}")]
    [Authorize(Roles = "Admin,Staff,Technician")]
    public async Task<IActionResult> GetTasksBySubscription(int subscriptionId)
    {
        var list = await _service.GetTasksBySubscriptionIdAsync(subscriptionId);
        return Ok(list);
    }
}
