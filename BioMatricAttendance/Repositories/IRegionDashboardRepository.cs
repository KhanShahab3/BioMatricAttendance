using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IRegionDashboardRepository
    {
        Task<List<Institute>> GetInstitutesAsync(int? regionId, int? districtId);
        Task<List<BiomatricDevice>> GetDevicesAsync(List<int> instituteIds);
        //Task<List<Candidate>> GetCandidatesAsync(List<int> deviceIds);
        //Task<List<TimeLogs>> GetTimeLogsAsync(
        //    List<int> deviceIds,
        //    DateTime fromUtc,
        //    DateTime toUtc);
        Task<List<Institute>> GetInstituteByDistrictId(int? instituteId, int districtId);
        Task<int> GetCourseCountAsync(List<int> instituteIds);
    }
}
