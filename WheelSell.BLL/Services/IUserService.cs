using WheelSell.DAL.Entities;

namespace WheelSell.BLL.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task AddUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}