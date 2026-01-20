using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Repositories
{
    public interface IDashboardRepository
    {
        Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int? regionId);
        Task<AttendanceDetailedReportDto> GetDetailedAttendanceReportAsync(
    int? regionId,
    DateTime? startDate,
    DateTime? endDate);
    }
}
