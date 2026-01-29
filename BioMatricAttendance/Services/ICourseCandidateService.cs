using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;

namespace BioMatricAttendance.Services
{
    public interface ICourseCandidateService
    {
        Task<CourseCandidateResponse> GetCouseInfoAsync(int courseId);
        Task AddCourseCandidates(CourseCandidateDto dto);
        Task<CandidateCourses?> GetById(int id);
        Task RemoveCourseCandidates(CandidateCourses entity);
    }
}
