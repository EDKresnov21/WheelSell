using Microsoft.AspNetCore.Mvc;
using WheelSell.BLL.Services;
using WheelSell.DAL.Entities;

namespace WheelSell.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly CarService _service;
        public CarsController(CarService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllCarsAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var car = await _service.GetCarByIdAsync(id);
            return car != null ? Ok(car) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Car car)
        {
            await _service.AddCarAsync(car);
            return Ok(car);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _service.DeleteCarAsync(id);
            return NoContent();
        }
    }
}
