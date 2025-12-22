using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IUserService
    {
        public Task<User> CreateUser(User user);
        public Task<User> GetUserById(int id);
        public Task<List<User>> GetUsers();
        public Task<User> UpdateUser(User user);
        public Task<bool> DeleteUser(int id);

    }
}
