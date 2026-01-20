using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
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
            //1.Get all devices for this institute

           List <BiomatricDevice> devices = await _appContext.BiomatricDevices
               .Where(d => d.InstituteId == instituteId)
               .ToListAsync();

            if (!devices.Any())
                    return new List<AttendanceReportDto>();

            var deviceIds = devices.Select(d => d.DeviceId).ToList();


            var rawData = await _appContext.TimeLogs
                .Where(l => deviceIds.Contains(l.DeviceId) &&
                            l.PunchTime >= startDate &&
                            l.PunchTime <= endDate)
                .ToListAsync();

            //            var rawData = await (
            //    from log in _appContext.TimeLogs
            //    join dev in _appContext.BiomatricDevices
            //        on log.DeviceId equals dev.DeviceId
            //    where dev.InstituteId == instituteId &&
            //          log.PunchTime >= startDate &&
            //          log.PunchTime <= endDate
            //    select log
            //).ToListAsync();





            if (!rawData.Any())
                return new List<AttendanceReportDto>();

          
            var candidateIds = rawData.Select(r => (int)r.DeviceUserId).Distinct().ToList();
            var candidates = await _appContext.Candidates
                .Where(c => candidateIds.Contains(c.Id) && deviceIds.Contains(c.DeviceId))
                .ToListAsync();

            
            var report = rawData
                .Where(r => candidates.Any(c => c.Id == r.DeviceUserId))
                .GroupBy(t => new { t.DeviceUserId, Date = t.PunchTime.Date })
                .Select(g => new
                {
                    CandidateId = (int)g.Key.DeviceUserId,
                    Date = g.Key.Date,
                    CheckIn = g.Where(x => x.AttendType == "DutyOn").Min(x => (DateTime?)x.PunchTime),
                    CheckOut = g.Where(x => x.AttendType == "DutyOff").Max(x => (DateTime?)x.PunchTime)
                })
                .Where(a => a.CheckIn.HasValue && a.CheckOut.HasValue)
                .Join(candidates,
                      att => att.CandidateId,
                      c => c.Id,
                      (att, c) => new AttendanceReportDto
                      {
                          CandidateId = c.Id,
                          CandidateName = c.Name,
                          CandidateType = c.Previliges,
                          Date = att.Date,
                          CheckIn = att.CheckIn.Value,
                          CheckOut = att.CheckOut.Value,
                          TotalHours = att.CheckOut.Value - att.CheckIn.Value
                      })
                .OrderBy(x => x.Date)
                .ThenBy(x => x.CandidateName)
                .ToList();

            return report;
        }
    }
}