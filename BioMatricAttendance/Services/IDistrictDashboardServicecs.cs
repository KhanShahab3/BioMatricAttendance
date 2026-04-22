using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IDistrictDashboardServicecs
    {
     Task<DistrictDashboardDto> GetDistrictDashboard(
int? districtId,
int? instituteId,
DateTime?startDate,
DateTime? endDate

);

        Task<DistrictDashboardReportDto> GetDashboardReportDto(
                   int? districtId,
                   DateTime ?startDate,
                   DateTime ?endTime,
                   int? instituteId);

    }
}
