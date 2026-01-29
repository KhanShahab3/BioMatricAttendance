using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Response;
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
            await using var transaction = await _context.Database.BeginTransactionAsync();

            try
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

                _instituteRepository.AddInstitute(institute);
                await _context.SaveChangesAsync();

                int instituteId = institute.Id;


                // can create function and pass institute Id and list of device ids and call repository here.
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
                await transaction.CommitAsync();

                return instituteId;

            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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

        public async Task<List<InstituteCoursesDto>> GetInstituteCourses()
        {
            var institutes=await _instituteRepository.GetInstituteCourses();
            var instDto = institutes.Select(ins => new InstituteCoursesDto
            {
                Id = ins.Id,
                InstituteName = ins.InstituteName,
                RegionName = new GetRegionNameDto
                {
                    RegionName = ins.Region.RegionName,
                },
                TotalCourses = ins.Courses?.Count() ?? 0
            }).ToList();
            return instDto;
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

       public async Task<List<BiomatricDevice>> GetInstituteWiseDevice(int InstituteId)
        {
            return await _instituteRepository.GetInstituteWiseDevice(InstituteId);
        }
       public async Task<List<InstituteCandidateResponse>> GetInstituteWiseCandidate(int InstituteId)
        {
            return await _instituteRepository.GetInstituteWiseCandidate(InstituteId);
        }
       public async Task<List<InstituteFacultyResponse>> GetInstituteWiseFaculty(int InstituteId)
        {
            return await _instituteRepository.GetInstituteWiseFaculty(InstituteId);
        }
       public async Task<List<InstitutePresentStudentResponse>> GetPresentStudentByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate)
        {
            return await _instituteRepository.GetPresentStudentByInstitute(InstituteId,StartDate,EndDate);
        }
        public async Task<List<InstitutePresentFaculityResponse>> GetPresentFaculityByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate)
        {
            return await _instituteRepository.GetPresentFaculityByInstitute(InstituteId, StartDate, EndDate );
        }
    }
}
