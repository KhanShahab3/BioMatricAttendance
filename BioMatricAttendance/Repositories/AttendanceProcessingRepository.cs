using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class AttendanceProcessingRepository : IAttendanceProcessingRepository
    {
        private readonly AppDbContext _appContext;

        public AttendanceProcessingRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }

        public async Task<List<AttendanceReportDto>> ProcessAttendance(
     DateTime startDate, DateTime endDate, long instituteId)
        {
            // Get all devices for this institute
            var devices = await _appContext.BiomatricDevices
                .Where(d => d.InstituteId == instituteId)
                .ToListAsync();

            if (!devices.Any())
            {
                return await GetExistingReport(instituteId, startDate, endDate);
            }

            var serialNumbers = devices.Select(d => d.DeviceId).ToList();

            // Get TimeLogs for all devices in this institute using deviceId
            var rawData = await _appContext.DeviceAttendanceLogs
         .Where(l => serialNumbers.Contains(l.DeviceId) &&  
                     l.PunchTime >= startDate &&
                     l.PunchTime <= endDate &&
                     !l.IsProcessed)
         .ToListAsync();

            if (!rawData.Any())
            {
                return await GetExistingReport(instituteId, startDate, endDate);
            }

            // Group by CandidateId and Date
            var attendanceList = rawData
                .GroupBy(t => new { t.DeviceUserId, Date = t.PunchTime.Date })
                .Select(g => new
                {
                    CandidateId = g.Key.DeviceUserId,
                    Date = g.Key.Date,
                    CheckIn = g.Where(x => x.AttendType == "DutyOn")
                               .Min(x => (DateTime?)x.PunchTime),
                    CheckOut = g.Where(x => x.AttendType == "DutyOff")
                                .Max(x => (DateTime?)x.PunchTime)
                })
                .ToList();

            // Save to Attendance table
            //foreach (var att in attendanceList)
            //{
            //    if (!att.CheckIn.HasValue || !att.CheckOut.HasValue)
            //        continue;

            //    var existingAttendance = await _appContext.Attendances
            //        .FirstOrDefaultAsync(a => a.CandidateId == att.CandidateId &&
            //                                  a.Date == att.Date);

            //    if (existingAttendance == null)
            //    {
            //        var attendance = new Attendance
            //        {
            //            CandidateId = att.CandidateId,
            //            Date = att.Date,
            //            CheckIn = att.CheckIn.Value,
            //            CheckOut = att.CheckOut.Value,
            //            InstituteId = att.InstituteId,
            //        };
            //        _appContext.Attendances.Add(attendance);
            //    }
            //}

            await _appContext.SaveChangesAsync();

            return await GetExistingReport(instituteId, startDate, endDate);
        }

        private async Task<List<AttendanceReportDto>> GetExistingReport(
            long instituteId, DateTime startDate, DateTime endDate)
        {
            var report = await _appContext.Attendances
                .Where(a => a.InstituteId == instituteId &&
                            a.Date >= startDate.Date &&
                            a.Date <= endDate.Date)
                .Join(_appContext.Candidates,
                      att => att.CandidateId,
                      person => person.Id,
                      (att, person) => new AttendanceReportDto
                      {
                          CandidateId = person.Id,
                          CandidateName = person.FullName,
                          CandidateType = person.CandidateType,
                          Date = att.Date,
                          CheckIn = att.CheckIn,
                          CheckOut = att.CheckOut,
                          TotalHours = att.CheckOut - att.CheckIn
                      })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.CandidateName)
                .ToListAsync();

            return report;
        }
    }
}