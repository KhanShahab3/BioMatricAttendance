using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Repositories
{
    public interface IDashboardRepository
    {
        Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int? regionId);
        Task<List<InstituteDashboardRowDto>> GetInstituteTableAsync(int? regionId);
    }
}
