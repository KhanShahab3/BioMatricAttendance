using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IInstituteService
    {
         Task<int> CreateInstitute(CreateInstituteDto instituteDto);
        Task<Institute> GetInstitute(int id);
         Task<List<Institute>> GetInstitutes();
        Task<Institute> UpdateInstitute(Institute institute);
         Task<bool> RemoveInstitute(int id);
    }
}
