using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }
        public async Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int regionId)
        {
            return await _dashboardRepository.GetSuperAdminDashboardAsync(regionId);
        }

        public async Task<List<InstituteDashboardRowDto>> InstituteTableAsync(int? regionId)
        {
            return await _dashboardRepository.GetInstituteTableAsync(regionId);
        }
    }
}
