using Microsoft.AspNetCore.Mvc;
using WheelSell.BLL.Services;
using WheelSell.BLL.DTO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace WheelSell.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly CarService _carService;

        public CarsController(CarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> GetCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            return Ok(cars);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> GetCar(int id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }

        [HttpPost]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> PostCar([FromBody] CarDTO carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var createdCar = await _carService.CreateCarAsync(carDto, userId);
            return CreatedAtAction("GetCar", new { id = createdCar.Id }, createdCar);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Saler,Admin")]
        public async Task<IActionResult> PutCar(int id, [FromBody] CarDTO carDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedCar = await _carService.UpdateCarAsync(id, carDto);
            if (updatedCar == null)
            {
                return NotFound();
            }
            return Ok(updatedCar);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCar(int id)
        {
            var result = await _carService.DeleteCarAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}