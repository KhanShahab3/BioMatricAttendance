using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IInstituteService
    {
         Task<int> CreateInstitute(CreateInstituteDto instituteDto);
        Task<GetInstituteDto> GetInstitute(int id);
         Task<List<GetInstituteDto>> GetInstitutes();
        Task<UpdateInstituteDto> UpdateInstitute(UpdateInstituteDto institute);
         Task<bool> RemoveInstitute(int id);
    }
}
