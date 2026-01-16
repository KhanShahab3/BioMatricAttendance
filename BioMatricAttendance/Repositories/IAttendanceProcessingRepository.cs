using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IAttendanceProcessingRepository
    {

        Task<AttendanceReportDto> GetAttendanceReport(DateTime startTime, DateTime endTime, long? deviceId = null);
    }
}
