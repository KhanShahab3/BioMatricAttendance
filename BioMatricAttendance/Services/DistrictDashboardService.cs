using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Helper;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class DistrictDashboardService : IDistrictDashboardServicecs
    {
        private readonly IRegionDashboardRepository _regionRepo;
        private readonly IInstituteAttendanceRepository _repo;

        public DistrictDashboardService(IRegionDashboardRepository regionRepo, IInstituteAttendanceRepository repo)
        {
            _regionRepo = regionRepo;
            _repo = repo;
        }


        public async Task<DistrictDashboardDto> GetDistrictDashboard(
        int? districtId,
        int? instituteId
       




            )
        {
            var (startUtc, endUtc) = DateTimeHelper.GetUtcRangeForPakistanDate(null, null);


            var institutes = await _regionRepo.GetInstituteByDistrictId(instituteId, districtId);

       

            //if (instituteId>0)
            //    institutes = institutes.Where(i => i.Id == instituteId.Value).ToList();

            var instituteIds = institutes.Select(i => i.Id).ToList();
            if (!instituteIds.Any())
                return new DistrictDashboardDto();

            var devices = await _regionRepo.GetDevicesAsync(instituteIds);
            var deviceIds = devices.Select(d => d.DeviceId).ToList();


            var candidates = await _repo.GetCandidatesByDeviceIds(deviceIds);
            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

            var logs = await _repo.GetTimeLogs(deviceIds, startUtc, endUtc);
            var presentIds = logs.Select(l => (int)l.DeviceUserId).Distinct().ToList();


            double dutyHours = 8;
            var facultyLogs = logs.Where(l => faculty.Any(f => f.DeviceUserId == l.DeviceUserId)).ToList();
  
            var summary = new DistrictDashboardSummaryDto
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

                StaffMaleCount = faculty.Count(f => f.gender == Gender.Male),
                StaffFemaleCount = faculty.Count(f => f.gender == Gender.Female),
                StudentMaleCount = students.Count(s => s.gender == Gender.Male),
                StudentFemaleCount = students.Count(s => s.gender == Gender.Female),

                TotalCourses = await _regionRepo.GetCourseCountAsync(instituteIds)
            };
  

            var instituteRows = new List<InstituteComparisonDto>();
            foreach (var institute in institutes)
            {
                var instDeviceIds = devices
                    .Where(d => d.InstituteId == institute.Id)
                    .Select(d => d.DeviceId)
                    .ToList();

                var instCandidates = candidates.Where(c => instDeviceIds.Contains(c.DeviceId)).ToList();
                var instFaculty = instCandidates.Where(c => c.Previliges == "Manager").ToList();
                var instStudents = instCandidates.Where(c => c.Previliges == "NormalUser").ToList();
                var instLogs = logs.Where(l => instDeviceIds.Contains(l.DeviceId)).ToList();


                var instFacultyLogs = instLogs.Where(l => instFaculty.Any(f => f.DeviceUserId == l.DeviceUserId)).ToList();


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

                    MaleCount = instCandidates.Count(c => c.gender == Gender.Male),
                    FemaleCount = instCandidates.Count(c => c.gender == Gender.Female),



                });
            }

            return new DistrictDashboardDto
            {
                Summary = summary,
                Institutes = instituteRows,
                
           
            };
        }



        public async Task<DistrictDashboardReportDto> GetDashboardReportDto(
            int ?districtId,
            DateTime ?startDate,
            DateTime ?endTime,
            int? instituteId = null)
        {
        
            var (startUtc, endUtc) = DateTimeHelper.GetUtcRangeForPakistanDate(startDate, endTime);


            var institutes = await _regionRepo.GetInstituteByDistrictId(instituteId, districtId);

            //if (instituteId.HasValue)
            //    institutes = institutes.Where(i => i.Id == instituteId.Value).ToList();

            var instituteIds = institutes.Select(i => i.Id).ToList();
            if (!instituteIds.Any())
                return new DistrictDashboardReportDto();

          
            var devices = await _regionRepo.GetDevicesAsync(instituteIds);
            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            var candidates = await _repo.GetCandidatesByDeviceIds(deviceIds);
            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

       
            var logs = await _repo.GetTimeLogs(deviceIds, startUtc, endUtc);
            var presentIds = logs.Select(l => (int)l.DeviceUserId).Distinct().ToList();

            var summary = new DistrictDashboardReportDto
            {
                TotalInstitutes = institutes.Count,
                TotalFaculty = faculty.Count,
                FacultyPresent = faculty.Count(f => presentIds.Contains(f.DeviceUserId)),
                FacultyAttendancePercentage = faculty.Any()
                    ? Math.Round((decimal)faculty.Count(f => presentIds.Contains(f.DeviceUserId)) / faculty.Count * 100, 1)
                    : 0,

                TotalStudents = students.Count,
                StudentsPresent = students.Count(s => presentIds.Contains(s.DeviceUserId)),
                StudentAttendancePercentage = students.Any()
                    ? Math.Round((decimal)students.Count(s => presentIds.Contains(s.DeviceUserId)) / students.Count * 100, 1)
                    : 0
            };

         
            var facultyReports = new List<DistrictFaculityAttendanceDto>();
            var studentReports = new List<DistrictStudentAttendanceDto>();

            foreach (var ins in institutes)
            {
                var instDeviceIds = devices
                    .Where(d => d.InstituteId == ins.Id)
                    .Select(d => d.DeviceId)
                    .ToList();

                var instCandidates = candidates.Where(c => instDeviceIds.Contains(c.DeviceId)).ToList();
                var instFaculty = instCandidates.Where(c => c.Previliges == "Manager").ToList();
                var instStudents = instCandidates.Where(c => c.Previliges == "NormalUser").ToList();

                var instPresentIds = logs
                    .Where(l => instDeviceIds.Contains(l.DeviceId))
                    .Select(l => (int)l.DeviceUserId)
                    .Distinct()
                    .ToList();

                facultyReports.Add(new DistrictFaculityAttendanceDto
                {
                    InstituteName = ins.InstituteName,
                    TotalFaculty = instFaculty.Count,
                    Present = instFaculty.Count(f => instPresentIds.Contains(f.DeviceUserId)),
                    //FacultyAbsent = instFaculty.Count - instFaculty.Count(f => instPresentIds.Contains(f.DeviceUserId)),
                    AttendancePercentage = instFaculty.Any()
                        ? Math.Round((decimal)instFaculty.Count(f => instPresentIds.Contains(f.DeviceUserId)) / instFaculty.Count * 100, 1)
                        : 0
                });

                studentReports.Add(new DistrictStudentAttendanceDto
                {
                    InstituteName = ins.InstituteName,
                    TotalStudents = instStudents.Count,
                    Present = instStudents.Count(s => instPresentIds.Contains(s.DeviceUserId)),
                    //StudentsAbsent = instStudents.Count - instStudents.Count(s => instPresentIds.Contains(s.DeviceUserId)),
                   AttendancePercentage = instStudents.Any()
                        ? Math.Round((decimal)instStudents.Count(s => instPresentIds.Contains(s.DeviceUserId)) / instStudents.Count * 100, 1)
                        : 0
                });
            }

            summary.FacultyAttendanceReport = facultyReports;
            summary.StudentAttendanceReport = studentReports;

            return summary;
        }
    }
}
    
    
    