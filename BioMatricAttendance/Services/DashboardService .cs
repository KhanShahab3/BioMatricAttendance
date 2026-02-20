using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{
    public class DashboardService:IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly AppDbContext _context;
        public DashboardService(IDashboardRepository dashboardRepository,AppDbContext context)
        {
            _dashboardRepository = dashboardRepository;
            _context = context;
        }
        public async Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int regionId)


        {

            var institutes = await _dashboardRepository.GetInstitutes(regionId);

            var instituteIds = institutes.Select(i => i.Id).ToList();
            var devices=await _dashboardRepository.GetDevices(instituteIds);
            var devicesIds = devices.Select(d => d.DeviceId).ToList();
            var candidates = await _dashboardRepository.GetCandidates(devicesIds);

            var faculityMale=candidates.Where(m=>m.gender== Models.Gender.Male&& m.Previliges=="Manager").Count();
            var faculityFemale=candidates.Where(f=>f.gender==Models.Gender.Female&& f.Previliges=="Manager").Count();   
            var studentMale=candidates.Where(s=>s.gender==Gender.Male&&s.Previliges== "NormalUser").Count();
            var studentFemale=candidates.Where(s=>s.gender==Gender.Female&&s.Previliges=="NormalUser").Count();

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

            var todayLogs= await _dashboardRepository.GetTodayLogs(devicesIds);
            var presentCandidateIds = todayLogs
            .Select(t => (int)t.DeviceUserId)
            .Distinct()
            .ToList();

            var facultyPresent = faculty.Count(f => presentCandidateIds.Contains(f.DeviceUserId));
            var studentsPresent = students.Count(s => presentCandidateIds.Contains(s.DeviceUserId));

            var activeDeviceIds = todayLogs.Select(t => t.DeviceId).Distinct().ToList();
            var devicesActive = devices.Count(d => activeDeviceIds.Contains(d.DeviceId));

            var totalFaculty = faculty.Count;
            var totalStudents = students.Count;
            var facultyPresentPct = totalFaculty > 0 ? (facultyPresent * 100 / totalFaculty) : 0;
            var facultyAbsentPct = 100 - facultyPresentPct;
            var studentsPresentPct = totalStudents > 0 ? (studentsPresent * 100 / totalStudents) : 0;
            var studentsAbsentPct = 100 - studentsPresentPct;

            var instituteRows = new List<InstituteDashboardRowDto>();

            foreach (var institute in institutes)
            {
                var instituteDevices = devices.Where(d => d.InstituteId == institute.Id).ToList();
                var instituteDeviceIds = instituteDevices.Select(d => d.DeviceId).ToList();

                var instituteCandidates = candidates.Where(c => instituteDeviceIds.Contains(c.DeviceId)).ToList();
                var instituteFaculty = instituteCandidates.Where(c => c.Previliges == "Manager").ToList();
                var instituteStudents = instituteCandidates.Where(c => c.Previliges == "NormalUser").ToList();

                var instituteTodayLogs = todayLogs.Where(t => instituteDeviceIds.Contains(t.DeviceId)).ToList();
                var institutePresentIds = instituteTodayLogs.Select(t => (int)t.DeviceUserId).Distinct().ToList();

                var instituteActiveDevices = instituteTodayLogs.Select(t => t.DeviceId).Distinct().Count();

                instituteRows.Add(new InstituteDashboardRowDto
                {
                    InstituteName = institute.InstituteName,
                    RegionName = institute.Region?.RegionName ?? "",
                    FacultyPresent = instituteFaculty.Count(f => institutePresentIds.Contains(f.DeviceUserId)),
                    TotalFaculty = instituteFaculty.Count,
                    StudentPresent = instituteStudents.Count(s => institutePresentIds.Contains(s.DeviceUserId)),
                    TotalStudent = instituteStudents.Count,
                    DevicesActive = instituteActiveDevices,
                    DevicesInactive = instituteDevices.Count - instituteActiveDevices
                });
            }

            return new SuperAdminDashboardDto
            {
                TotalInstitutes = institutes.Count,
                TotalFaculty = totalFaculty,
                MaleFaculityCount = faculityMale,
                FemaleFaculityCount = faculityFemale,
                MaleStudentCount = studentMale,
                FemaleStudentCount = studentFemale,
                TotalFacultyPresent = facultyPresent,
                TotalStudents = totalStudents,
                TotalStudentsPresent = studentsPresent,
                TotalDevices = devices.Count,
                TotalDevicesActive = devicesActive,
                TotalRegisteredCourses = await _context.Courses.CountAsync(c => !c.IsDeleted && instituteIds.Contains(c.InstituteId)),
                FacultyPresentPercentage = facultyPresentPct,
                FacultyAbsentPercentage = facultyAbsentPct,
                StudentsPresentPercentage = studentsPresentPct,
                StudentsAbsentPercentage = studentsAbsentPct,
                InstituteDashboardRows = instituteRows
            };
        }
        public async Task<AttendanceDetailedReportDto> GetAttendanceReportAsync(
  int? regionId,
  DateTime? startDate,
  DateTime? endDate)
        {
            var institutes = await _dashboardRepository.GetInstitutes(regionId);
                var instituteIds = institutes.Select(i => i.Id).ToList();
            var devices = await _dashboardRepository.GetDevices(instituteIds);
            var deviceIds=devices.Select(d => d.DeviceId).ToList();
                var candidates = await _dashboardRepository.GetCandidates(deviceIds);
                var candidateIds=candidates.Select(c => c.Id).ToList();
                var logs = await _dashboardRepository.GetLogs(deviceIds, startDate, endDate);

            var presentCandidateIds = logs
        .Select(t => (int)t.DeviceUserId)
        .Distinct()
        .ToList();
            var facultyReport = new List<InstituteAttendanceRowDto>();
            foreach (var institute in institutes)
            {
                var instituteDeviceIds = devices
                    .Where(d => d.InstituteId == institute.Id)
                    .Select(d => d.DeviceId)
                    .ToList();

                var instituteFaculty = candidates
                    .Where(c => instituteDeviceIds.Contains(c.DeviceId) &&
                               c.Previliges == "Manager")
                    .ToList();

                var totalFaculty = instituteFaculty.Count;
                var facultyPresent = instituteFaculty.Count(f => presentCandidateIds.Contains(f.DeviceUserId));
                var facultyAbsent = totalFaculty - facultyPresent;
                var attendancePct = totalFaculty > 0 ? (decimal)facultyPresent / totalFaculty * 100 : 0;

                facultyReport.Add(new InstituteAttendanceRowDto
                {
                    InstituteName = institute.InstituteName,
                    Region = institute.Region?.RegionName ?? "",
                    Total = totalFaculty,
                    Present = facultyPresent,
                    Absent = facultyAbsent,
                    AttendancePercentage = Math.Round(attendancePct, 1),
                    Status = GetAttendanceStatus(attendancePct)
                });
            }
            var studentReport = new List<InstituteAttendanceRowDto>();

            foreach (var institute in institutes)
            {
                var instituteDeviceIds = devices
                    .Where(d => d.InstituteId == institute.Id)
                    .Select(d => d.DeviceId)
                    .ToList();

                var instituteStudents = candidates
                    .Where(c => instituteDeviceIds.Contains(c.DeviceId) &&
                               c.Previliges == "NormalUser")
                    .ToList();

                var totalStudents = instituteStudents.Count;
                var studentsPresent = instituteStudents.Count(s => presentCandidateIds.Contains(s.DeviceUserId));
                var studentsAbsent = totalStudents - studentsPresent;
                var attendancePct = totalStudents > 0 ? (decimal)studentsPresent / totalStudents * 100 : 0;

                studentReport.Add(new InstituteAttendanceRowDto
                {
                    InstituteName = institute.InstituteName,
                    Region = institute.Region?.RegionName ?? "",
                    Total = totalStudents,
                    Present = studentsPresent,
                    Absent = studentsAbsent,
                    AttendancePercentage = Math.Round(attendancePct, 1),
                    Status = GetAttendanceStatus(attendancePct)
                });
            }
            var allFaculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var allStudents = candidates.Where(c => c.Previliges == "NormalUser").ToList();

            var totalFacultyCount = allFaculty.Count;
            var totalStudentsCount = allStudents.Count;
            var facultyPresentCount = allFaculty.Count(f => presentCandidateIds.Contains(f.DeviceUserId));
            var studentsPresentCount = allStudents.Count(s => presentCandidateIds.Contains(s.DeviceUserId));

            var summary = new AttendanceReportSummaryDto
            {
                TotalInstitutes = institutes.Count,
                TotalFaculty = totalFacultyCount,
                FacultyPresent = facultyPresentCount,
                FacultyAttendancePercentage = totalFacultyCount > 0
         ? Math.Round((decimal)facultyPresentCount / totalFacultyCount * 100, 1)
         : 0,
                TotalStudents = totalStudentsCount,
                StudentsPresent = studentsPresentCount,
                StudentAttendancePercentage = totalStudentsCount > 0
         ? Math.Round((decimal)studentsPresentCount / totalStudentsCount * 100, 1)
         : 0,
                //StartDate = startPk,
                //EndDate = endPk
            };

            return new AttendanceDetailedReportDto
            {
                Summary = summary,
                FacultyReport = facultyReport.OrderByDescending(f => f.AttendancePercentage).ToList(),
                StudentReport = studentReport.OrderByDescending(s => s.AttendancePercentage).ToList()
            };
        }

        private string GetAttendanceStatus(decimal percentage)
        {
            if (percentage >= 80) return "Good";
            if (percentage >= 60) return "Average";
            return "Poor";
        }
    }
        //public async Task<List<InstituteDashboardRowDto>> InstituteTableAsync(int? regionId)
        //{
        //    return await _dashboardRepository.GetInstituteTableAsync(regionId);
        //}
    }

