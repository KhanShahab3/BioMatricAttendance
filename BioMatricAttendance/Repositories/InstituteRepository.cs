using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class InstituteRepository:IInstituteRepository
    {
        private readonly AppDbContext _appContext;
        public InstituteRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        public async Task<Institute> AddInstitute(Institute institute)
        {
            await _appContext.Institutes.AddAsync(institute);
            await _appContext.SaveChangesAsync();
            return institute;
        }
        public async Task<Institute> GetInstituteById(int id)
        {
            var institute = await _appContext.Institutes.FindAsync(id);
            return institute;
        }
        public async Task<List<Institute>> GetAllInstitutes()
        {
            var institutes = await _appContext.Institutes.ToListAsync();
            return institutes;
        }
        public async Task<Institute> UpdateInstitute(Institute institute)
        {
            var isInstitute = await _appContext.Institutes.FindAsync(institute.Id);
            if (isInstitute != null)
            {
                isInstitute.InstituteName = institute.InstituteName;
                isInstitute.Address = institute.Address;
                isInstitute.ContactNumber = institute.ContactNumber;
                await _appContext.SaveChangesAsync();
            }
            return institute;
        }
        public async Task<bool> DeleteInstitute(int id)
        {
            var isInstitute = await _appContext.Institutes.FindAsync(id);
            if (isInstitute != null)
            {
                _appContext.Remove(isInstitute);
                await _appContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
