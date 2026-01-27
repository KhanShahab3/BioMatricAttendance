using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IInstituteRepository
    {
        Task AddInstitute(Institute institute);
        Task<Institute> GetInstituteById(int id);
         Task<List<Institute>> GetAllInstitutes();
         Task<Institute> UpdateInstitute(Institute institute);
         Task<bool> DeleteInstitute(int id);
        Task<List<Institute>> GetInstituteCourses();

        Task<string?> GetInstituteName(int instituteId);
    }
}
