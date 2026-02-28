using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;

namespace BioMatricAttendance.Services
{
    public interface IInstituteService
    {
         Task<int> CreateInstitute(CreateInstituteDto instituteDto);
        Task<GetInstituteDto> GetInstitute(int id);
         Task<List<GetInstituteDto>> GetInstitutes();
        Task<UpdateInstituteDto> UpdateInstitute(UpdateInstituteDto institute);
         Task<bool> RemoveInstitute(int id);
        Task<List<InstituteCoursesDto>> GetInstituteCourses();
        Task<List<BiomatricDevice>> GetInstituteWiseDevice(int InstituteId);
        Task<List<InstituteCandidateResponse>> GetInstituteWiseCandidate(int InstituteId);
        Task<List<InstituteFacultyResponse>> GetInstituteWiseFaculty(int InstituteId);
        Task<List<InstitutePresentStudentResponse>> GetPresentStudentByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate);
        Task<List<InstitutePresentFaculityResponse>> GetPresentFaculityByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate);
        Task<PagedResult<GetInstituteDto>> GetInstitutesPaged(int page, int pageSize);

    }
}
