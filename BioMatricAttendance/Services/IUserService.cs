using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IUserService
    {
        public Task<User> CreateUser(User user);
        public Task<User> GetUserById(int id);
        public Task<List<User>> GetUsers();
        public Task<UpdateUserDTO> UpdateUser(UpdateUserDTO user);
        public Task<bool> DeleteUser(int id);

    }
}
