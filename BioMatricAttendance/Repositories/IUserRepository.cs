using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IUserRepository
    {
      public Task<User> AddUser(User user);
        public Task<User> GetUserById(int id);
        public Task<List<User>> GetAllUsers();
        public Task<UpdateUserDTO> UpdateUser(UpdateUserDTO user);
        public Task<bool> DeleteUser(int id);
    }
}
