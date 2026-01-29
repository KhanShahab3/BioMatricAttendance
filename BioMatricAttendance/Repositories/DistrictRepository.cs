using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class DistrictRepository:IDistrictRepository
    {
        private readonly AppDbContext _appContext;
        public DistrictRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }   
        public async Task<List<District>> GetAllDistricts(int regionId)
        {
            return await _appContext.Districts
                .Where(x=>x.RegionId == regionId)
                .ToListAsync();
        }
    }
}
