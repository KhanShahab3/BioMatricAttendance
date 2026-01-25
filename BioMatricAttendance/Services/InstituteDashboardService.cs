using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
 
    public class InstituteDashboardService : IInstituteDashboardService
    {
        private readonly IInstituteAttendanceRepository _instituteRepository;
        private readonly ICourseRepository _courseRepository;
        public InstituteDashboardService(IInstituteAttendanceRepository instituteRepository, ICourseRepository courseRepository)
        {
            _instituteRepository = instituteRepository;
            _courseRepository = courseRepository;
        }

        public async Task<InstituteDashboardDto> GetInstituteDashboard(int instituteId)
        {
            
            var today = DateTime.UtcNow.AddHours(5).Date;
            var fromUtc = today.AddHours(-5);
            var toUtc = fromUtc.AddDays(1);

        
            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);

            var deviceIds = devices.Select(d => d.DeviceId).ToList();
            var totalDevices = deviceIds.Count;

            if (totalDevices == 0)
            {
                return new InstituteDashboardDto
                {
                  InstituteName = string.Empty,
                };
            }

           
            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

          
            var logs = await _instituteRepository.GetTimeLogs(deviceIds, fromUtc, toUtc);

            var presentUserIds = logs
                .Select(l => (int)l.DeviceUserId)
                .Distinct()
                .ToList();

           
            var activeDevices = logs
                .Select(l => l.DeviceId)
                .Distinct()
                .Count();

           
            var courseCount = await _instituteRepository.GetCourseCountByInstituteId(instituteId);

          
            return new InstituteDashboardDto
            {
                InstituteName = "Test Institute",
                TotalFaculty = faculty.Count,
                FacultyPresent = faculty.Count(f => presentUserIds.Contains(f.DeviceUserId)),


                

                TotalStudents = students.Count,
                StudentsPresent = students.Count(s => presentUserIds.Contains(s.DeviceUserId)),

                TotalCourses = courseCount
            };
        }


        public async Task<List<CourseAttendanceDto>> GetCourseWiseAttendanceAsync(int instituteId)
        {
           
            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);

            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            if (!deviceIds.Any())
                return new List<CourseAttendanceDto>();

           
            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);
            var students = candidates
                .Where(c => c.Previliges == "NormalUser")
                .ToList();

          
            const int PakistanUtcOffset = 5;
            var todayUtc = DateTime.UtcNow
                .AddHours(PakistanUtcOffset)
                .Date
                .AddHours(-PakistanUtcOffset);

            var tomorrowUtc = todayUtc.AddDays(1);

           
            var todayLogs = await _instituteRepository
                .GetTimeLogs(deviceIds, todayUtc, tomorrowUtc);

            var presentStudentIds = todayLogs
                .Select(t => (int)t.DeviceUserId)
                .Distinct()
                .ToHashSet();

           
            var courses = await _courseRepository.GetCourseByInstituteId(instituteId);

          
            var result = new List<CourseAttendanceDto>();

            foreach (var course in courses)
            {
                var courseStudents = students
                    .Where(s => s.CourseId == course.Id)
                    .ToList();

                var total = courseStudents.Count;

                var present = courseStudents
                    .Count(s => presentStudentIds.Contains(s.DeviceUserId));

                var absent = total - present;

                var percentage = total == 0
                    ? 0
                    : Math.Round((decimal)present * 100 / total, 1);

                result.Add(new CourseAttendanceDto
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    TotalStudents = total,
                    PresentStudents = present,
                    AbsentStudents = absent,
                    AttendancePercentage = percentage
                });
            }

            return result;
        }

        public async Task<InstituteAttendanceReportDto> GetAttendanceReportAsync(int instituteId, DateTime from, DateTime to)
        {
            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);

            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            if (!deviceIds.Any())
                return null;

            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

            var logs = await _instituteRepository
                     .GetTimeLogs(deviceIds, from, to);
            var presentUserIds = logs.Select(l => (int)l.DeviceUserId).Distinct().ToHashSet();

            
            var facultySummary = new FacultyAttendanceSummaryDto
            {
                TotalFaculty = faculty.Count,
                Present = faculty.Count(f => presentUserIds.Contains(f.DeviceUserId)),
                Absent = faculty.Count(f => !presentUserIds.Contains(f.DeviceUserId)),
                //OnLeave = faculty.Count(f => f.IsOnLeave),
                //Overtime = logs.Count(l => l.IsOvertime && faculty.Select(f => f.DeviceUserId).Contains(l.DeviceUserId))
            };

            var studentPresent = students.Count(s => presentUserIds.Contains(s.DeviceUserId));
            var studentTotal = students.Count;

            var studentSummary = new StudentAttendanceSummaryDto
            {
                TotalStudents = studentTotal,
                Present = studentPresent,
                Absent = studentTotal - studentPresent,
                AttendanceRate = studentTotal == 0 ? 0 : Math.Round((decimal)studentPresent * 100 / studentTotal, 1)
            };

          
            
            var courses = await _courseRepository.GetCourseByInstituteId(instituteId);

            var courseWiseAttendance = new List<CourseAttendanceDto>();
            foreach (var course in courses)
            {
                var courseStudents = students.Where(s => s.CourseId == course.Id).ToList();
                var total = courseStudents.Count;
                var present = courseStudents.Count(s => presentUserIds.Contains(s.DeviceUserId));
                var absent = total - present;
                var percentage = total == 0 ? 0 : Math.Round((decimal)present * 100 / total, 1);

                courseWiseAttendance.Add(new CourseAttendanceDto
                {
                    CourseId = course.Id,
                    CourseName = course.CourseName,
                    TotalStudents = total,
                    PresentStudents = present,
                    AbsentStudents = absent,
                    AttendancePercentage = percentage,
                    //Status = percentage >= 75 ? "Good" : "Poor"
                });
            }

            return new InstituteAttendanceReportDto
            {
                FacultySummary = facultySummary,
                StudentSummary = studentSummary,
                CourseWiseAttendance = courseWiseAttendance
            };
        }


    }
}
