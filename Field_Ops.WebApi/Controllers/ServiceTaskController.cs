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

    public ServiceTasksController(IServiceTasksService service, IEmployeesRepository empRepo)
    {
        _service = service;
        _empRepo = empRepo;
    }

    [HttpPost]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Create(ServiceTaskCreateDto dto)
    {
        dto.ActionUserId = User.GetUserId();
        var response = await _service.CreateAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAll()
    {
        var response = await _service.GetAllAsync();
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin,Staff,Technician")]
    public async Task<IActionResult> GetById(int id)
    {
        var response = await _service.GetByIdAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Staff,Admin")]
    public async Task<IActionResult> Update(int id, ServiceTaskUpdateDto dto)
    {
        dto.Id = id;
        dto.ActionUserId = User.GetUserId();

        var response = await _service.UpdateAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpPatch("{id:int}/status")]
    [Authorize(Roles = "Staff,Technician,Admin")]
    public async Task<IActionResult> UpdateStatus(int id, ServiceTaskUpdateStatusDto dto)
    {
        dto.Id = id;
        dto.ActionUserId = User.GetUserId();

        var response = await _service.UpdateStatusAsync(dto);
        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("awaiting-approval")]
    [Authorize(Roles = "Admin,Staff")]
    public async Task<IActionResult> GetAwaitingApproval()
    {
        var response = await _service.GetAwaitingApprovalAsync(User.GetUserId());
        return StatusCode(response.StatusCode, response);
    }
}
