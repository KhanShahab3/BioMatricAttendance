using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class RegionRepository:IRegionRepository
    {
        private readonly AppDbContext _appContext;

        public RegionRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }   

        public async Task<List<Region>> GetAllRegions()
        {
            return await _appContext.Regions
                .ToListAsync();
        }

    }
}
