using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using System.Threading.Tasks;

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

       Task<(List<Institute> Items, int TotalCount)> GetInstitutePaged(int page, int pageSize);
        Task<string?> GetInstituteName(int? instituteId);
        Task<List<BiomatricDevice>> GetInstituteWiseDevice(int InstituteId);
        Task<List<InstituteCandidateResponse>> GetInstituteWiseCandidate(int InstituteId);
        Task<List<InstituteFacultyResponse>> GetInstituteWiseFaculty(int InstituteId);
        Task<List<InstitutePresentStudentResponse>> GetPresentStudentByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate);
        Task<List<InstitutePresentFaculityResponse>> GetPresentFaculityByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate);
    }
}
