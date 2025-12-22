using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IInstituteRepository
    {
        public Task<Institute> AddInstitute(Institute institute);
        public Task<Institute> GetInstituteById(int id);
        public Task<List<Institute>> GetAllInstitutes();
        public Task<Institute> UpdateInstitute(Institute institute);
        public Task<bool> DeleteInstitute(int id);
    }
}
