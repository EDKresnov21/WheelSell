using Microsoft.AspNetCore.Mvc;
using WheelSellTA.BLL.Services;
using WheelSellTA.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace WheelSell.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class TransmissionsController : ControllerBase
    {
        private readonly TransmissionService _transmissionService;

        public TransmissionsController(TransmissionService transmissionService)
        {
            _transmissionService = transmissionService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTransmissions()
        {
            var transmissions = await _transmissionService.GetAllTransmissionsAsync();
            return Ok(transmissions);
        }

        [HttpPost]
        public async Task<IActionResult> PostTransmission([FromBody] TransmissionDTO transmissionDto)
        {
            var transmission = await _transmissionService.CreateTransmissionAsync(transmissionDto);
            return CreatedAtAction("GetTransmission", new { id = transmission.Id }, transmission);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTransmission(int id, [FromBody] TransmissionDTO transmissionDto)
        {
            var updatedTransmission = await _transmissionService.UpdateTransmissionAsync(id, transmissionDto);
            if (updatedTransmission == null)
            {
                return NotFound();
            }
            return Ok(updatedTransmission);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTransmission(int id)
        {
            var result = await _transmissionService.DeleteTransmissionAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}