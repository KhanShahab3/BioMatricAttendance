using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;

namespace BioMatricAttendance.Repositories
{
    public interface ICourseCandidatesRepository
    {
        Task<List<CandidateCourses>> GetCandidateByCourseId(int courseId);
        Task<CourseCandidateResponse> GetCouseInfoAsync(int courseId);
        Task AddCourseCandidates(CourseCandidateDto dto);
        Task RemoveCourseCandidates(CandidateCourses entity);
        Task<CandidateCourses?> GetById(int id);


    }
}
