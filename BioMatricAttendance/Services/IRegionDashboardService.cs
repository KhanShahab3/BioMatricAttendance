using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IRegionDashboardService
    {
        Task<RegionDashboardDto> GetRegionDashboardAsync(
       int regionId,
       int? districtId);
      
    }
}
