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
        public async Task<int> CreateInstitute(CreateInstituteDto dto)
        {
            
            var institute = new Institute
            {
                InstituteName = dto.InstituteName,
                Address = dto.Address,
                ContactNumber = dto.ContactNumber,
                Email = dto.Email,
                ContactPerson = dto.ContactPerson,
                RegionId = dto.RegionId,
                CreatedAt = dto.CreatedAt
            };

            _context.Institutes.Add(institute);
            await _context.SaveChangesAsync(); 

            int instituteId = institute.Id; 

           
            var devices = await _context.BiomatricDevices
                .Where(d => dto.DeviceIds.Contains(d.Id))
                .ToListAsync();

            
            if (devices.Count != dto.DeviceIds.Count)
                throw new Exception("One or more devices not found");

            if (devices.Any(d => d.InstituteId != null))
                throw new Exception("Device already assigned to another institute");

          
            foreach (var device in devices)
            {
                device.InstituteId = instituteId;
            }

           
            await _context.SaveChangesAsync();

            return instituteId;
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
                Id = OneInstiute.Id,
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
                Id = institute.Id,
                InstituteName = institute.InstituteName,
                Address = institute.Address,
                ContactNumber = institute.ContactNumber,
                Email = institute.Email,
                ContactPerson = institute.ContactPerson,
                CreatedAt = institute.CreatedAt,
                Region = new GetRegionNameDto
                {
                    Id=institute.Region.Id,
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
