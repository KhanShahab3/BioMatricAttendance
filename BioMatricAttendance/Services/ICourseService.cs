using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Response;

namespace BioMatricAttendance.Services
{
    public interface ICourseService
    {
        Task<List<GetCourseDto>>GetCoursesByInstituteId(int instituteId);

        Task<CourseDto> AddCourseAsync(CourseDto dto);
        Task<CourseDto> UpdateCourseAsync(CourseDto dto);
        Task<bool> DeleteCourseAsync(int courseId);
        Task<List<CourseDto>> GetAllCoursesAsync();
    }
}
