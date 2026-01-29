using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Helper;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;


namespace BioMatricAttendance.Services
{
    public class RegionDashboardService : IRegionDashboardService
    {
     private readonly IRegionDashboardRepository _regionRepo;
        private readonly IInstituteAttendanceRepository _repo;

        public RegionDashboardService(IRegionDashboardRepository regionRepo,IInstituteAttendanceRepository repo)
        {
            _regionRepo = regionRepo;
            _repo= repo;
        }

        public async Task<RegionDashboardDto> GetRegionDashboardAsync(
            int regionId,
            int? districtId
            )
        {
         

            var (startUtc, endUtc) = DateTimeHelper.GetUtcRangeForPakistanDate(null, null);



            var institutes = await _regionRepo.GetInstitutesAsync(regionId, districtId);
            var instituteIds = institutes.Select(i => i.Id).ToList();

            if (!instituteIds.Any())
                return new RegionDashboardDto();

          
            var devices = await _regionRepo.GetDevicesAsync(instituteIds);
            var deviceIds = devices.Select(d => d.DeviceId).ToList();

          
            var candidates = await _repo.GetCandidatesByDeviceIds(deviceIds);

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

         
            var logs = await _repo.GetTimeLogs(deviceIds,  startUtc,endUtc);
            var presentIds = logs.Select(l => (int)l.DeviceUserId).Distinct().ToList();

           
            var summary = new RegionDashboardSummaryDto
            {
                TotalInstitutes = institutes.Count,
                TotalFaculty = faculty.Count,
                FacultyPresent = faculty.Count(f => presentIds.Contains(f.DeviceUserId)),
                FacultyAttendancePercentage =
                    faculty.Any()
                        ? Math.Round((decimal)faculty.Count(f => presentIds.Contains(f.DeviceUserId)) / faculty.Count * 100, 1)
                        : 0,

                TotalStudents = students.Count,
                StudentsPresent = students.Count(s => presentIds.Contains(s.DeviceUserId)),
                StudentAttendancePercentage =
                    students.Any()
                        ? Math.Round((decimal)students.Count(s => presentIds.Contains(s.DeviceUserId)) / students.Count * 100, 1)
                        : 0,

                TotalDevices = devices.Count,
                ActiveDevices = logs.Select(l => l.DeviceId).Distinct().Count(),

                StaffMaleCount = candidates.Count(c => c.gender == Gender.Male && c.Previliges=="Manager"),
                StaffFemaleCount = candidates.Count(c => c.gender == Gender.Female && c.Previliges == "Manager"),

                StudentMaleCount = candidates.Count(c => c.gender == Gender.Male && c.Previliges == "NormalUser"),
                StudentFemaleCount = candidates.Count(c => c.gender == Gender.Female && c.Previliges == "NormalUser"),

                TotalCourses = await _regionRepo.GetCourseCountAsync(instituteIds)
            };

          
            var instituteRows = new List<InstituteComparisonDto>();

            foreach (var institute in institutes)
            {
                var instDeviceIds = devices
                    .Where(d => d.InstituteId == institute.Id)
                    .Select(d => d.DeviceId)
                    .ToList();

                var instCandidates = candidates
                    .Where(c => instDeviceIds.Contains(c.DeviceId))
                    .ToList();

                if (!instCandidates.Any()) continue;

                var instFaculty = instCandidates.Where(c => c.Previliges == "Manager").ToList();
                var instStudents = instCandidates.Where(c => c.Previliges == "NormalUser").ToList();

                var instLogs = logs.Where(l => instDeviceIds.Contains(l.DeviceId)).ToList();

                instituteRows.Add(new InstituteComparisonDto
                {
                    InstituteName = institute.InstituteName,
                    DistrictName = institute.District?.DistrictName ?? "",

                    TotalFaculty = instFaculty.Count,
                    FacultyPresent = instFaculty.Count(f => presentIds.Contains(f.DeviceUserId)),
                    FacultyAttendancePercentage =
                        instFaculty.Any()
                            ? Math.Round((decimal)instFaculty.Count(f => presentIds.Contains(f.DeviceUserId)) / instFaculty.Count * 100, 1)
                            : 0,

                    TotalStudents = instStudents.Count,
                    StudentsPresent = instStudents.Count(s => presentIds.Contains(s.DeviceUserId)),
                    StudentAttendancePercentage =
                        instStudents.Any()
                            ? Math.Round((decimal)instStudents.Count(s => presentIds.Contains(s.DeviceUserId)) / instStudents.Count * 100, 1)
                            : 0,

                    TotalDevices = instDeviceIds.Count,
                    ActiveDevices = instLogs.Select(l => l.DeviceId).Distinct().Count(),

                    MaleCount = candidates.Count(c => c.gender == Gender.Male),
                    FemaleCount = candidates.Count(c => c.gender == Gender.Female),
                });
            }

            return new RegionDashboardDto
            {
                //RegionId = regionId,
                //DistrictId = districtId,
                //Date = reportDate,
                Summary = summary,
                Institutes = instituteRows
            };
        }
    }

}
