using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>>GetCourseByInstituteId(int ?instituteId);

        Task<Course>CreateCourse(Course course);
        Task<List<Course>> GetAllCourses();
        Task<bool> DeleteCourse(int courseId);

        Task<Course> UpdateCourse(Course course);
    }
}
