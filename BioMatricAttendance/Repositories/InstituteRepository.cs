using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class InstituteRepository:IInstituteRepository
    {
        private readonly AppDbContext _appContext;
        public InstituteRepository(AppDbContext appDbContext)
        {
            _appContext = appDbContext;
        }
        public async Task AddInstitute(Institute institute)
        {
            await _appContext.Institutes.AddAsync(institute);
          
        }
        public async Task<Institute?> GetInstituteById(int id)
        {
            var institute = await _appContext.Institutes
                .Include(i => i.Region)
                .Include(i => i.BiomatricDevices)
                .Where(i => i.IsDeleted == false) 
                .FirstOrDefaultAsync(i => i.Id == id);

            return institute;
        }
        public async Task<List<Institute>> GetAllInstitutes()
        {
            var institutes = await _appContext.Institutes
                .Include(i => i.Region)
                .Include(i => i.BiomatricDevices)
                .Where(i => i.IsDeleted == false)
                .ToListAsync();
            return institutes;
        }

        public async Task<List<Institute>> GetInstituteCourses()
        {
            var instituteCourses = await _appContext.Institutes
                .Include(ic => ic.Region)
                .Include(ic => ic.Courses)
                .Where(ic => ic.IsDeleted == false)
                .ToListAsync();
            return instituteCourses;
        }
        public async Task<Institute> UpdateInstitute(Institute institute)
        {
            var isInstitute = await _appContext.Institutes.FindAsync(institute.Id);
            if (isInstitute != null)
            {
                isInstitute.InstituteName = institute.InstituteName;
                isInstitute.Address = institute.Address;
                isInstitute.ContactNumber = institute.ContactNumber;
                isInstitute.Email = institute.Email;
                isInstitute.ContactPerson = institute.ContactPerson;
                isInstitute.UpdatedAt = DateTime.UtcNow;
                isInstitute.RegionId = institute.RegionId;
                await _appContext.SaveChangesAsync();
            }
            return institute;
        }
        public async Task<bool> DeleteInstitute(int id)
        {
            var institute = await _appContext.Institutes.FindAsync(id);

            if (institute != null)
            {
               
                institute.IsDeleted = true;

              
                _appContext.Institutes.Update(institute);

                
                await _appContext.SaveChangesAsync();
                return true;
            }

            return false;
        }

        public async Task<string?> GetInstituteName(int instituteId)
        {
            return await _appContext.Institutes
                .Where(i => i.Id == instituteId && !i.IsDeleted)
                .Select(i => i.InstituteName)
                .FirstOrDefaultAsync();
        }
         public async Task<List<BiomatricDevice>> GetInstituteWiseDevice(int InstituteId)
        {
           return await _appContext.BiomatricDevices.Where(x=>x.InstituteId==InstituteId&& x.isRegistered).ToListAsync();
        }
        public async Task<List<InstituteCandidateResponse>> GetInstituteWiseCandidate(int InstituteId) {
        
          
         var devcies= await _appContext.BiomatricDevices.Where(x => x.InstituteId == InstituteId && x.isRegistered).ToListAsync();
            var devicesIds = devcies.Select(i => i.DeviceId).ToList();

            var candidates = await _appContext.Candidates
     .Where(s => devicesIds.Contains(s.DeviceId) && s.Previliges == "NormalUser")
     .Select(s => new InstituteCandidateResponse
     {
         Id = s.Id,
         Name = s.Name,
         gender = s.gender.Value
     })
     .ToListAsync();
            return candidates;
        }
      public async  Task<List<InstituteFacultyResponse>> GetInstituteWiseFaculty(int InstituteId)
        {
            var devcies = await _appContext.BiomatricDevices.Where(x => x.InstituteId == InstituteId && x.isRegistered).ToListAsync();
            var devicesIds = devcies.Select(i => i.DeviceId).ToList();

            var facilities = await _appContext.Candidates
     .Where(s => devicesIds.Contains(s.DeviceId) && s.Previliges=="Manager")
     .Select(s => new InstituteFacultyResponse
     {
         Id = s.Id,
         Name = s.Name,
         gender = s.gender.Value
     })
     .ToListAsync();
            return facilities;
        }

        public async Task<List<InstitutePresentStudentResponse>> GetPresentStudentByInstitute(int InstituteId, DateTime StartDate, DateTime EndDate)
        {
            var devicIds=await _appContext.BiomatricDevices.Where(x=>x.InstituteId== InstituteId).Select(x => x.DeviceId).ToListAsync();

            var pakistanTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Karachi");
            var result = await (
               from t in _appContext.TimeLogs
               join c in _appContext.Candidates
                   on t.DeviceUserId equals c.DeviceUserId
               where devicIds.Contains(t.DeviceId)
                     && c.Previliges == "NormalUser"
                     && t.PunchTime >= DateTime.SpecifyKind(StartDate, DateTimeKind.Utc)
                     && t.PunchTime < DateTime.SpecifyKind(EndDate, DateTimeKind.Utc)
               group t by new
               {
                   t.DeviceUserId,
                   t.DeviceId,
                   c.Name,
                   PunchDate = t.PunchTime.Date   // 👈 KEY CHANGE
               }
               into g
               select new InstitutePresentStudentResponse
               {
                   DeviceUserId = g.Key.DeviceUserId,
                   DeviceId = g.Key.DeviceId,
                   StudentName = g.Key.Name,
                   PunchDate = g.Key.PunchDate.Date,   // add this property
                   FirstPunch = g.Min(x => x.PunchTime).ToString("HH:mm:ss"),
                   LastPunch = g.Max(x => x.PunchTime).ToString("HH:mm:ss")
               }
           ).ToListAsync();

            return result;
        }
        public async Task<List<InstitutePresentFaculityResponse>> GetPresentFaculityByInstitute(int InstituteId,DateTime StartDate,DateTime EndDate)
        {
            var devicIds = await _appContext.BiomatricDevices.Where(x => x.InstituteId == InstituteId).Select(x => x.DeviceId).ToListAsync();

            

            var result = await (
                from t in _appContext.TimeLogs
                join c in _appContext.Candidates
                    on t.DeviceUserId equals c.DeviceUserId
                where devicIds.Contains(t.DeviceId)
                      && c.Previliges == "Manager"
                     && t.PunchTime >= DateTime.SpecifyKind(StartDate, DateTimeKind.Utc)
                     && t.PunchTime < DateTime.SpecifyKind(EndDate, DateTimeKind.Utc)
                group t by new
                {
                    t.DeviceUserId,
                    t.DeviceId,
                    c.Name,
                    PunchDate = t.PunchTime.Date   // 👈 KEY CHANGE
                }
                into g
                select new InstitutePresentFaculityResponse
                {
                    DeviceUserId = g.Key.DeviceUserId,
                    DeviceId = g.Key.DeviceId,
                    FaculityName = g.Key.Name,
                    PunchDate = g.Key.PunchDate.Date,   // add this property
                    FirstPunch = g.Min(x => x.PunchTime).ToString("HH:mm:ss"),
                    LastPunch = g.Max(x => x.PunchTime).ToString("HH:mm:ss")
                }
            ).ToListAsync();

            return result;
        }
    }
}
