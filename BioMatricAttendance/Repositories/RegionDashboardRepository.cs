using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class RegionDashboardRepository: IRegionDashboardRepository
    {
        private readonly AppDbContext _context;
        public RegionDashboardRepository(AppDbContext context)
        {
            _context = context;
        }





        public async Task<List<Institute>> GetInstitutesAsync(int regionId, int? districtId)
        {
            var query = _context.Institutes
                .Include(i => i.District)
                .Where(i => i.RegionId == regionId && !i.IsDeleted);

            if (districtId.HasValue)
            {
                query = query.Where(i => i.DistrictId == districtId.Value);
            }

            return await query.ToListAsync();
        }

        public async Task<List<BiomatricDevice>> GetDevicesAsync(List<int> instituteIds)
        {
            return await _context.BiomatricDevices
                .Where(d => instituteIds.Contains(d.InstituteId) && !d.IsDeleted)
                .ToListAsync();
        }

        //public async Task<List<Candidate>> GetCandidatesAsync(List<int> deviceIds)
        //{
        //    return await _context.Candidates
        //        .Where(c => deviceIds.Contains(c.DeviceId) && c.Enable)
        //        .ToListAsync();
        //}

        //public async Task<List<TimeLogs>> GetTimeLogsAsync(
        //    List<int> deviceIds,
        //    DateTime fromUtc,
        //    DateTime toUtc)
        //{
        //    return await _context.TimeLogs
        //        .Where(t =>
        //            deviceIds.Contains(t.DeviceId) &&
        //            t.PunchTime >= fromUtc &&
        //            t.PunchTime < toUtc &&
        //            t.DeviceUserId > 0)
        //        .ToListAsync();
        //}

        public async Task<int> GetCourseCountAsync(List<int> instituteIds)
        {
            return await _context.Courses
                .CountAsync(c => instituteIds.Contains(c.InstituteId) && !c.IsDeleted);
        }

    }
}
