using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IDashboardRepository
    {

        Task<List<Institute>> GetInstitutes(int? regionId);
        Task<List<BiomatricDevice>> GetDevices(List<int> instituteIds);
        Task<List<Candidate>> GetCandidates(List<long> deviceIds);
        Task<List<TimeLogs>> GetTodayLogs(List<long> deviceIds);
        Task<List<TimeLogs>> GetLogs(List<long> devicesIds, DateTime? startDate, DateTime? endDate);
        //Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int? regionId);
    //    Task<AttendanceDetailedReportDto> GetDetailedAttendanceReportAsync(
    //int? regionId,
    //DateTime? startDate,
    //DateTime? endDate);
    }
}
