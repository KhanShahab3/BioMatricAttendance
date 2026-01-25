using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface ICourseRepository
    {
        Task<List<Course>>GetCourseByInstituteId(int instituteId);
    }
}
