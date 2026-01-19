using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IAttendanceProcessingRepository
    {

        Task<List<AttendanceReportDto>> ProcessAttendance(
      DateTime startDate, DateTime endDate, long instituteId);
    }
}
