using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class InstituteAttendanceRepository: IInstituteAttendanceRepository
    {
        private readonly AppDbContext _context;
        public InstituteAttendanceRepository(AppDbContext context)
        {

            _context = context;
        }

        public async Task<List<BiomatricDevice>> GetDevicesByInstituteId(int? instituteId)
        {
            var devices=await _context.BiomatricDevices
                .Where(d=>d.InstituteId==instituteId && d.isRegistered && !d.IsDeleted)
                .ToListAsync();
            return devices;
        }

        public async Task<List<Candidate>> GetCandidatesByDeviceIds(List<long> deviceIds)
        {
            var canaidates=await _context.Candidates
                .Where(c=>deviceIds.Contains(c.DeviceId))
                .ToListAsync();
            return canaidates;
        }
        public async Task<List<TimeLogs>> GetTimeLogs(List<long> deviceIds, DateTime from, DateTime to)
        {
            var rawLogs=await _context.TimeLogs
                .Where(t=>deviceIds.Contains(t.DeviceId)&&
                t.PunchTime>=from&&
                t.PunchTime<to
                )
                .ToListAsync();
            return rawLogs;
        }

        public async Task<int> GetCourseCountByInstituteId(int ?instituteId)
        {
            return await _context.Courses
               .CountAsync(c => c.InstituteId == instituteId &&

               !c.IsDeleted);



        }
    }
}
