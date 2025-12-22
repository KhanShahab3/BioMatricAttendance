using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class UserService:IUserService

    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<User>CreateUser(User user)
        {
            return await _userRepository.AddUser(user);
        }
        public async Task<User> GetUserById(int id)
        {
            return await _userRepository.GetUserById(id);
        }
        public async Task<List<User>> GetUsers()
        {
            return await _userRepository.GetAllUsers();
        }
        public async Task<User> UpdateUser(User user)
        {
            return await _userRepository.UpdateUser(user);
        }
        public async Task<bool> DeleteUser(int id)
        {
            return await _userRepository.DeleteUser(id);
        }
    }
}
