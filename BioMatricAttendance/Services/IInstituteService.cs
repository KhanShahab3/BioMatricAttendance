using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IInstituteService
    {
        public Task<Institute> CreateInstitute(Institute institute);
        public Task<Institute> GetInstitute(int id);
        public Task<List<Institute>> GetInstitutes();
        public Task<Institute> UpdateInstitute(Institute institute);
        public Task<bool> RemoveInstitute(int id);
    }
}
