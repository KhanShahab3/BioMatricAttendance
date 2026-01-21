using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllRegions();
    }
}
