using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AppDbContext _appContext;
        public UserRepository(AppDbContext appDbContext ) {
            _appContext = appDbContext;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var users = await _appContext.Users.ToListAsync();
            return users;
        }
        public async Task<User> AddUser(User user)
        {
            
                await _appContext.Users.AddAsync(user);
               await _appContext.SaveChangesAsync();
                return user;
        }
         public async Task<User> GetUserById(int id)
        {
            var user = await _appContext.Users.FindAsync(id);
            return user;
        }
        public async Task<User> UpdateUser(User user)
        {
            try
            {
                var isUser = await _appContext

                    .Users.FindAsync(user.Id);

                if (isUser != null)
                {
                    isUser.Name = user.Name;
                    isUser.Email = user.Email;
                    isUser.Password = user.Password;
                }
                //_appDbContext.Update(user);
                await _appContext.SaveChangesAsync();
                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

        }
        public async Task<bool> DeleteUser(int id)
        {
            var isUser = await _appContext.Users.FindAsync(id);
            if (isUser != null)
            {
                _appContext.Remove(isUser);
                await _appContext.SaveChangesAsync();

                return true;
            }
            return false;

        }

    }
}
