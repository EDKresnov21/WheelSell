using Microsoft.EntityFrameworkCore;
using WheelSell.DAL;
using WheelSell.DAL.Entities;

namespace WheelSell.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context) => _context = context;

        public async Task<List<User>> GetAllUsersAsync() => await _context.Users.ToListAsync();
        public async Task<User?> GetUserByIdAsync(int id) => await _context.Users.FindAsync(id);
        public async Task AddUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }
    }
}