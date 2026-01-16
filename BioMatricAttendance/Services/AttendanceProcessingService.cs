using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class AttendanceProcessingService:IAttendanceProcessingService
    {
        private readonly IAttendanceProcessingRepository _attendanceProcessingRepository;
        public AttendanceProcessingService(IAttendanceProcessingRepository attendanceProcessingRepository)
        {
            _attendanceProcessingRepository = attendanceProcessingRepository;
        }

        public async Task<AttendanceReportDto> ProcessFacultyAttendanceAsync(DateTime startDate, DateTime endDate, long deviceId)
        {
         var logs=await _attendanceProcessingRepository.GetAttendanceReport(startDate, endDate, deviceId);
            return logs;

        }
    }
}
