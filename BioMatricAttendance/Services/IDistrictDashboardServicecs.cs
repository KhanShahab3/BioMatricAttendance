using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IDistrictDashboardServicecs
    {
     Task<DistrictDashboardDto> GetDistrictDashboard(
int districtId,
int? instituteId = null,
     int pageNumber = 1,
        int pageSize = 10
);

        Task<DistrictDashboardReportDto> GetDashboardReportDto(
                   int districtId,
                   DateTime ?startDate,
                   DateTime ?endTime,
                   int? instituteId);

    }
}
