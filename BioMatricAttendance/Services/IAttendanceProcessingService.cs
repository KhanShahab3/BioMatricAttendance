using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IAttendanceProcessingService
    {
        Task<List<AttendanceReportDto>> ProcessFacultyAttendanceAsync(
        DateTime startDate, DateTime endDate, int instituteId);
    }
}
