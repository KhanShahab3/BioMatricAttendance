using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IInstituteDashboardService
    {
        Task<InstituteDashboardDto> GetInstituteDashboard(int instituteId);
        Task<List<CourseAttendanceDto>> GetCourseWiseAttendanceAsync(int instituteId);

        Task<InstituteAttendanceReportDto> GetAttendanceReportAsync(int instituteId, DateTime ?startDate, DateTime ?endDate);
    }
}
