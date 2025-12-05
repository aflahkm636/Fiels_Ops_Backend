using Field_Ops.Application.Contracts.Service;
using Field_Ops.Application.DTO.EmployeeDto;
using Field_Ops.Application.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Field_Ops.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeesService _service;

        public EmployeesController(IEmployeesService service)
        {
            _service = service;
        }
        

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Admin,Staff")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] EmployeeUpdateDto dto)
        {
            dto.ModifiedBy=User.GetUserId();
            var result = await _service.UpdateEmployeeAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("{id}")]

        public async Task<IActionResult> Delete(int id)
        {
            int deletedBy = User.GetUserId();
            var result = await _service.DeleteEmployeeAsync(id, deletedBy);
            return StatusCode(result.StatusCode, result);
        }
    }
}
