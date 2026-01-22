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
        public async Task<GetInstituteDto> GetInstitute(int id)
        {
            var OneInstiute= await _instituteRepository.GetInstituteById(id);
            if (OneInstiute == null)
            {
                return null;
            }
            var instituteDto = new GetInstituteDto
            {
                InstituteName = OneInstiute.InstituteName,
                Address = OneInstiute.Address,
                ContactNumber = OneInstiute.ContactNumber,
                Email = OneInstiute.Email,
                ContactPerson = OneInstiute.ContactPerson,
                CreatedAt = OneInstiute.CreatedAt,
                Region = new GetRegionNameDto
                {
                    RegionName = OneInstiute.Region.RegionName
                },
                DeviceCount = OneInstiute.BiomatricDevices?.Count() ?? 0
            };
            return instituteDto;

        }
        
        public async Task<List<GetInstituteDto>> GetInstitutes()


        {
          
            var institutes= await _instituteRepository.GetAllInstitutes();

            var instituteDtos = institutes.Select(institute => new GetInstituteDto
            {
                InstituteName = institute.InstituteName,
                Address = institute.Address,
                ContactNumber = institute.ContactNumber,
                Email = institute.Email,
                ContactPerson = institute.ContactPerson,
                CreatedAt = institute.CreatedAt,
                Region = new GetRegionNameDto
                {
                    RegionName = institute.Region.RegionName
                },
                DeviceCount = institute.BiomatricDevices?.Count() ?? 0
            }).ToList();
            return instituteDtos;

        }
        public async Task<UpdateInstituteDto> UpdateInstitute(UpdateInstituteDto institute)
        {
            var UpdateInstitute=new Institute
            {
                Id = institute.Id,
                InstituteName = institute.InstituteName,
                Address = institute.Address,
                ContactNumber = institute.ContactNumber,
                Email = institute.Email,
                ContactPerson = institute.ContactPerson,
                RegionId = institute.RegionId,
                UpdatedAt = institute.UpdatedAt
            };
            await _instituteRepository.UpdateInstitute(UpdateInstitute);
            return institute;
        }
        public async Task<bool> RemoveInstitute(int id)
        {
            return await _instituteRepository.DeleteInstitute(id);
        }
    }
}
