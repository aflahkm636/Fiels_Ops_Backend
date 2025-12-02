using Field_Ops.Application.Contracts.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Field_Ops.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechniciansController : ControllerBase
    {
        private readonly ITechniciansService _service;

        public TechniciansController(ITechniciansService service)
        {
            _service = service;
        }
        [Authorize(Roles ="Staff")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("active")]
        public async Task<IActionResult> GetActive()
        {
            var result = await _service.GetActiveAsync();
            return StatusCode(result.StatusCode, result);
        }

        [Authorize(Roles = "Staff")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}
