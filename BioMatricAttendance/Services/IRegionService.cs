using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IRegionService
    {
        Task<List<GetRegionNameDto>> GetRegions();
    }
}
