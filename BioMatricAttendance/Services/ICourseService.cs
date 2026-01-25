using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface ICourseService
    {
        Task<List<GetCourseDto>>GetCoursesByInstituteId(int instituteId);
    }
}
