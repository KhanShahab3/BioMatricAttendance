using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IAttendanceProcessingService
    {
        Task<AttendanceReportDto> ProcessFacultyAttendanceAsync(DateTime startDate, DateTime endDate, long deviceId);
    }
}
