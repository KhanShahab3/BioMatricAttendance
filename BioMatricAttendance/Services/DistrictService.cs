using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class DistrictService: IDistrictService
    {
      private readonly IDistrictRepository _districtRepository;
        public DistrictService(IDistrictRepository districtRepository)
        {
            _districtRepository = districtRepository;
        }
        public async Task<List<DistrictDto>> GetDistrict()
        {
            var districts= await _districtRepository.GetAllDistricts();
            var districtDtos=districts.Select(d=>new DistrictDto
            {
                Id=d.Id,
                DistrictName=d.DistrictName
            }).ToList();
            return districtDtos;
        }


    }
}
