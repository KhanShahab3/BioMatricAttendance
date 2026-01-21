using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{
    public class InstituteService:IInstituteService
    {
        private readonly IInstituteRepository _instituteRepository;
        private readonly IBioMatricDeviceRepository _deviceRepository;
        private readonly AppDbContext _context;
        public InstituteService(IInstituteRepository instituteRepository, AppDbContext context, IBioMatricDeviceRepository deviceRepository)
        {
            _instituteRepository = instituteRepository;
            _context = context;
            _deviceRepository = deviceRepository;
        }
        public async Task<int> CreateInstitute(CreateInstituteDto instituteDto)
        {
            var institute = new Institute
            {
                InstituteName = instituteDto.InstituteName,
                Address = instituteDto.Address,
                ContactNumber = instituteDto.ContactNumber,
                Email = instituteDto.Email,
                ContactPerson = instituteDto.ContactPerson,
                RegionId = instituteDto.RegionId,
               
                CreatedAt = instituteDto.CreatedAt
            };  
             await _instituteRepository.AddInstitute(institute);
            await _context.SaveChangesAsync();

            foreach (var deviceId in instituteDto.DeviceIds)
            {
                var device = await _deviceRepository.GetDeviceById(deviceId);
                if (device != null)
                {
                    
                    device.InstituteId = institute.Id;
                    _deviceRepository.UpdateDevice(device);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception("Device not found");
                }
            }


            return institute.Id;

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
