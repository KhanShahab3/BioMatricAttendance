using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IDashboardService
    {
        Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int regionId);
        //Task<List<InstituteDashboardRowDto>> InstituteTableAsync(int? regionId);
        Task<AttendanceDetailedReportDto> GetAttendanceReportAsync(
  int? regionId,
  DateTime? startDate,
  DateTime? endDate);
    }
}
