using WheelSell.DAL.Entities;

namespace WheelSell.BLL.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetAllCarsAsync();
        Task<Car?> GetCarByIdAsync(int id);
        Task AddCarAsync(Car car);
        Task DeleteCarAsync(int id);
    }
}