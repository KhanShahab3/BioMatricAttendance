using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class RegionService:IRegionService
    {
        private readonly IRegionRepository _regionRepository;

        public RegionService(IRegionRepository regionRepository)
        {
            _regionRepository = regionRepository;
        }   

        public async Task<List<GetRegionNameDto>> GetRegions()
        {
            var regions = await _regionRepository.GetAllRegions();

            return regions.Select(r => new GetRegionNameDto
            {
                Id = r.Id,
                RegionName = r.RegionName
            }).ToList();    

        }
    }
}
