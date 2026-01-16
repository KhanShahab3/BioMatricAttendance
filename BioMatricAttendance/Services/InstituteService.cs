using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class InstituteService:IInstituteService
    {
        private readonly IInstituteRepository _instituteRepository;
        public InstituteService(IInstituteRepository instituteRepository)
        {
            _instituteRepository = instituteRepository;
        }
        public async Task<Institute> CreateInstitute(Institute institute)
        {
            return await _instituteRepository.AddInstitute(institute);
        }
        public async Task<Institute> GetInstitute(int id)
        {
            return await _instituteRepository.GetInstituteById(id);
        }
        
        public async Task<List<Institute>> GetInstitutes()
        {
            return await _instituteRepository.GetAllInstitutes();
        }
        public async Task<Institute> UpdateInstitute(Institute institute)
        {
            return await _instituteRepository.UpdateInstitute(institute);
        }
        public async Task<bool> RemoveInstitute(int id)
        {
            return await _instituteRepository.DeleteInstitute(id);
        }
    }
}
