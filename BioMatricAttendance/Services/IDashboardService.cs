using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IDashboardService
    {
        Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int regionId,DateTime ?startDate,DateTime? endDate);
        //Task<List<InstituteDashboardRowDto>> InstituteTableAsync(int? regionId);
        Task<AttendanceDetailedReportDto> GetAttendanceReportAsync(
  int? regionId,
  DateTime? startDate,
  DateTime? endDate);
    }
}
