using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IDistrictService
    {
        Task<List<DistrictDto>> GetDistrict(int regionId);
    }
}
