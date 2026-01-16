using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class AttendanceProcessingRepository:IAttendanceProcessingRepository
    {
        private readonly AppDbContext _appContext;
        public AttendanceProcessingRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }

        //working hours=8


        public async Task<AttendanceReportDto> GetAttendanceReport(DateTime startTime, DateTime endTime, long? deviceId = null)
        {
           
            var logsQuery = _appContext.DeviceAttendanceLogs
                .Where(l => l.PunchTime >= startTime && l.PunchTime <= endTime);

            if (deviceId.HasValue)
                logsQuery = logsQuery.Where(l => l.DeviceId == deviceId.Value);

            var logs = await logsQuery.ToListAsync();
            
            Console.WriteLine($"Total logs fetched: {logs.Count}");


            var students = await _appContext.Students.ToListAsync();

            Console.WriteLine($"Total students fetched: {students}");


            var studentAttendanceRecords = logs
                .Where(l => students.Any(s => s.BiometricId == l.DeviceId)) 
                .GroupBy(l => l.DeviceId)
                .Select(g =>
                {
                    var student = students.First(s => s.BiometricId == g.Key);

                    int presentCount = g.Count(l => l.AttendType == "DutyOn");
                    int absentCount = g.Count(l => l.AttendType == "DutyOff");
                    int total = presentCount + absentCount;

                    double percentagePresent = total > 0 ? (presentCount * 100.0 / total) : 0;

                    return new AttendanceRecordDTO
                    {
                        PersonId = student.Id,
                        PersonName = student.FirstName,
                        TotalPresent = presentCount,
                        TotalAbsent = absentCount,
                        PercentagePresent = Math.Round(percentagePresent, 2)
                    };
                })
                .ToList();

           
            var report = new AttendanceReportDto
            {
                From = startTime,
                To = endTime,
                StudentRecords = studentAttendanceRecords
            };

            return report;
        }


    }
}
