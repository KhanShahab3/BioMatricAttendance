using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IDistrictRepository
    {
        Task<List<District>> GetAllDistricts();
    }

}
