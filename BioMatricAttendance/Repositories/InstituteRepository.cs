using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
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
        public async Task AddInstitute(Institute institute)
        {
            await _appContext.Institutes.AddAsync(institute);
          
        }
        public async Task<Institute?> GetInstituteById(int id)
        {
            var institute = await _appContext.Institutes
                .Include(i => i.Region)
                .Include(i => i.BiomatricDevices)
                .Where(i => i.IsDeleted == false) 
                .FirstOrDefaultAsync(i => i.Id == id);

            return institute;
        }
        public async Task<List<Institute>> GetAllInstitutes()
        {
            var institutes = await _appContext.Institutes
                .Include(i => i.Region)
                .Include(i => i.BiomatricDevices)
                .Where(i => i.IsDeleted == false)
                .ToListAsync();
            return institutes;
        }

        public async Task<List<Institute>> GetInstituteCourses()
        {
            var instituteCourses = await _appContext.Institutes
                .Include(ic => ic.Region)
                .Include(ic => ic.Courses)
                .Where(ic => ic.IsDeleted == false)
                .ToListAsync();
            return instituteCourses;
        }
        public async Task<Institute> UpdateInstitute(Institute institute)
        {
            var isInstitute = await _appContext.Institutes.FindAsync(institute.Id);
            if (isInstitute != null)
            {
                isInstitute.InstituteName = institute.InstituteName;
                isInstitute.Address = institute.Address;
                isInstitute.ContactNumber = institute.ContactNumber;
                isInstitute.Email = institute.Email;
                isInstitute.ContactPerson = institute.ContactPerson;
                isInstitute.UpdatedAt = DateTime.UtcNow;
                isInstitute.RegionId = institute.RegionId;
                await _appContext.SaveChangesAsync();
            }
            return institute;
        }
        public async Task<bool> DeleteInstitute(int id)
        {
            var institute = await _appContext.Institutes.FindAsync(id);

            if (institute != null)
            {
               
                institute.IsDeleted = true;

              
                _appContext.Institutes.Update(institute);

                
                await _appContext.SaveChangesAsync();
                return true;
            }

            return false;
        }
    }
}
