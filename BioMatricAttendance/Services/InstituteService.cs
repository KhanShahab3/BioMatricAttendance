using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Helper;
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


        public async Task<CandidateAttendanceHistoryDto?> GetCandidateAttendanceHistoryAsync(
    int instituteId,
    int candidateId,
    DateTime? startDatePk,
    DateTime? endDatePk
    )
        {
            var (startDate, endDate) = DateTimeHelper.GetUtcRangeForPakistanDate(startDatePk, endDatePk);

           
            var candidate = await _context.Candidates
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == candidateId && c.Enable);
            if (candidate == null) return null;

           
            var device = await _context.BiomatricDevices
                .AsNoTracking()
                .FirstOrDefaultAsync(d => d.DeviceId == candidate.DeviceId && d.InstituteId == instituteId && !d.IsDeleted && d.isRegistered);
            if (device == null) return null;

           
            var logs = await _context.TimeLogs
                .AsNoTracking()
                .Where(t => t.DeviceId == candidate.DeviceId
                            && t.DeviceUserId == candidate.DeviceUserId
                            && t.PunchTime >= startDate
                            && t.PunchTime < endDate)
                .ToListAsync();

          
            var totalDays = (endDate.Date - startDate.Date).Days;
            var days = Enumerable.Range(0, totalDays)
                .Select(i =>
                {
                    var date = startDate.Date.AddDays(i);
                    var dayLogs = logs.Where(l => l.PunchTime.Date == date).ToList();
                    if (!dayLogs.Any())
                        return new DailyAttendanceDto { Date = date, Present = false };
                    var first = dayLogs.Min(x => x.PunchTime);
                    var last = dayLogs.Max(x => x.PunchTime);
                    return new DailyAttendanceDto
                    {
                        Date = date,
                        Present = true,
                        FirstPunch = first.ToString("HH:mm:ss"),
                        LastPunch = last.ToString("HH:mm:ss")
                    };
                }).ToList();

            return new CandidateAttendanceHistoryDto
            {
                CandidateId= candidate.Id,
                DeviceUserId = candidate.DeviceUserId,
                DeviceId = candidate.DeviceId,
                StartDate= startDate,
                EndDate= endDate,
                Name = candidate.Name ?? string.Empty,
                Previliges=candidate.Previliges,
                Days = days
            };
        }
        public async Task<int> CreateInstitute(CreateInstituteDto dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var institute = new Institute
                {
                    InstituteName = dto.InstituteName,
                    Address = dto.Address,
                    ContactNumber = dto.ContactNumber,
                    DistrictId = dto.DistrictId,
                    Email = dto.Email,
                    ContactPerson = dto.ContactPerson,
                    RegionId = dto.RegionId,
                    CreatedAt = dto.CreatedAt
                };

                _instituteRepository.AddInstitute(institute);
                await _context.SaveChangesAsync();

                var instituteId = institute.Id;

              
                if (dto.DeviceIds != null && dto.DeviceIds.Any())
                {
                    var selectedDeviceIds = dto.DeviceIds.Distinct().ToList();

                    var devices = await _context.BiomatricDevices
                        .Where(d => selectedDeviceIds.Contains(d.Id) && !d.IsDeleted)
                        .ToListAsync();

                    if (devices.Count != selectedDeviceIds.Count)
                        throw new InvalidOperationException("One or more selected devices were not found.");

                    // Prevent assigning devices already assigned to another institute
                    var alreadyAssigned = devices.Where(d => d.InstituteId.HasValue).ToList();
                    if (alreadyAssigned.Any())
                        throw new InvalidOperationException("One or more selected devices are already assigned to an institute.");

                    foreach (var device in devices)
                    {
                        device.InstituteId = instituteId;
                        device.isRegistered = true;
                    }

                    await _context.SaveChangesAsync();
                }

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
                DeviceCount = OneInstiute.BiomatricDevices?.Count() ?? 0,
                    Devices = OneInstiute.BiomatricDevices?.Select(d => new GetDeviceNameDto
                    {
                        Id = d.Id,
                        DeviceId = d.DeviceId,

                    }).ToList() ?? new List<GetDeviceNameDto>()
            };
            return instituteDto;

        }
        public async Task<PagedResult<GetInstituteDto>> GetInstitutesPaged(int page, int pageSize)
        {
            var (institutes, totalCount) = await _instituteRepository.GetInstitutePaged(page, pageSize);
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
            return new PagedResult<GetInstituteDto>
            {
                Items = instituteDtos,
                TotalCount = totalCount
            };
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
              
                DeviceCount = institute.BiomatricDevices?.Count() ?? 0,
                    Devices = institute.BiomatricDevices?.Select(d => new GetDeviceNameDto
                    {
                        Id = d.Id,
                        DeviceId = d.DeviceId,
    
                    }).ToList() ?? new List<GetDeviceNameDto>()


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
            if (institute == null) throw new ArgumentNullException(nameof(institute));

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var updateEntity = new Institute
                {
                    Id = institute.Id,
                    InstituteName = institute.InstituteName,
                    Address = institute.Address,
                    ContactNumber = institute.ContactNumber,
                    Email = institute.Email,
                    DistrictId = institute.DistrictId,
                    ContactPerson = institute.ContactPerson,
                    RegionId = institute.RegionId,
                    UpdatedAt = institute.UpdatedAt
                };

                
                await _instituteRepository.UpdateInstitute(updateEntity);

              
                if (institute.DeviceIds != null)
                {
                    var selectedDeviceIds = institute.DeviceIds;

                 
                    var devicesToAssign = await _context.BiomatricDevices
                        .Where(d => selectedDeviceIds.Contains(d.Id) && !d.IsDeleted)
                        .ToListAsync();

                    //if (devicesToAssign.Count != selectedDeviceIds.Count)
                    //    throw new InvalidOperationException("One or more devices not found.");

                   
                    var alreadyAssignedElsewhere = devicesToAssign
                        .Where(d => d.InstituteId.HasValue && d.InstituteId.Value != institute.Id)
                        .ToList();

                    if (alreadyAssignedElsewhere.Any())
                        throw new InvalidOperationException("One or more devices already assigned to another institute.");

                  
                    var currentlyAssigned = await _context.BiomatricDevices
                        .Where(d => d.InstituteId == institute.Id)
                        .ToListAsync();

                    foreach (var d in currentlyAssigned)
                    {
                        if (!selectedDeviceIds.Contains(d.Id))
                            d.InstituteId = null;
                    }

                  
                    foreach (var d in devicesToAssign)
                    {
                        d.InstituteId = institute.Id;
                        d.isRegistered = true;
                    }

                    await _context.SaveChangesAsync();
                }

                await transaction.CommitAsync();
                return institute;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
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
       public async Task<List<InstitutePresentStudentResponse>> GetPresentStudentByInstitute(int InstituteId, DateTime? StartDate, DateTime? EndDate)
        {
            return await _instituteRepository.GetPresentStudentByInstitute(InstituteId,StartDate,EndDate);
        }
        public async Task<List<InstitutePresentFaculityResponse>> GetPresentFaculityByInstitute(int InstituteId, DateTime? StartDate, DateTime ?EndDate)
        {
            return await _instituteRepository.GetPresentFaculityByInstitute(InstituteId, StartDate, EndDate );
        }
    }
}
