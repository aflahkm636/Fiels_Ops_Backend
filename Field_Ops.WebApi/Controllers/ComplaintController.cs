using Field_Ops.Application.Contracts.Repository;
using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO;
using Field_Ops.Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("api/[controller]")]
[ApiController]
public class ComplaintsController : ControllerBase
{
    private readonly IComplaintsService _service;
    private readonly ICustomerRepository _customerRepository;

    public ComplaintsController(IComplaintsService service ,ICustomerRepository customerRepository)
    {
        _service = service;
        _customerRepository = customerRepository;
    }

    [Authorize(Policy = "CustomerOnly")]
    [HttpPost]
    public async Task<IActionResult> Create(ComplaintCreateDto dto)
    {
        try
        {
            int userId = User.GetUserId();
            dto.CustomerId=await _customerRepository.GetCustomerIdByUserId(userId);
            dto.ActionUserId =userId;
            var result = await _service.CreateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { success = false, message = ex.Message });
        }
    }

    [Authorize(Roles ="Admin,Staff")]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { success = false, message = ex.Message });
        }
    }

    [Authorize(Roles = "Admin,Staff")]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { success = false, message = ex.Message });
        }
    }

    [Authorize(Roles = "Technician,Staff")]
    [HttpPut]
    public async Task<IActionResult> Update(ComplaintUpdateDto dto)
    {
        try
        {
            dto.ActionUserId = User.GetUserId();
            var result = await _service.UpdateAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { success = false, message = ex.Message });
        }
    }


    [Authorize(Roles = "Technician,Staff")]
    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus(ComplaintStatusUpdateDto dto)
    {
        try
        {
            dto.ActionUserId = User.GetUserId();
            //dto.ActionUserId = 3;
            var result = await _service.UpdateStatusAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { success = false, message = ex.Message });
        }
    }

    [Authorize(Policy ="CustomerOnly")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            int actionUserId = User.GetUserId();
            var result = await _service.DeleteAsync(id, actionUserId);
            return StatusCode(result.StatusCode, result);
        }
        catch (Exception ex)
        {
            return StatusCode(400, new { success = false, message = ex.Message });
        }
    }
}
