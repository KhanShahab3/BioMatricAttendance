using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
 
    public class InstituteDashboardService : IInstituteDashboardService
    {
        private readonly IInstituteAttendanceRepository _instituteRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstituteRepository _insRepo;
        public InstituteDashboardService(IInstituteAttendanceRepository instituteRepository, 
            ICourseRepository courseRepository,
            IInstituteRepository insRepo)
        {
            _instituteRepository = instituteRepository;
            _courseRepository = courseRepository;
            _insRepo = insRepo;
        }

        public async Task<InstituteDashboardDto> GetInstituteDashboard(int instituteId)
        {
            //var today = DateTime.UtcNow.AddHours(5).Date;
            //var fromUtc = today.AddHours(-5);
            //var toUtc = fromUtc.AddDays(1);



            const int PakistanUtcOffset = 5;

            // Pakistan today
            var todayPk = DateTime.UtcNow.AddHours(PakistanUtcOffset).Date;

            // Convert to UTC range
            var startUtc = todayPk.AddHours(-PakistanUtcOffset);
            var endUtc = startUtc.AddDays(1);





        




            var instituteName = await _insRepo.GetInstituteName(instituteId);

          
            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);
            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            if (!deviceIds.Any())
            {
                return new InstituteDashboardDto
                {
                    InstituteName = instituteName ?? string.Empty
                };
            }

        
            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

           
            var logs = await _instituteRepository.GetTimeLogs(deviceIds, startUtc, endUtc);

            var presentUserIds = logs
                .Select(l => (int)l.DeviceUserId)
                .Distinct()
                .ToHashSet(); 

          
            var facultyPresent = faculty.Count(f => presentUserIds.Contains(f.DeviceUserId));
            var studentPresent = students.Count(s => presentUserIds.Contains(s.DeviceUserId));

            var facultyAbsent = faculty.Count - facultyPresent;
            var studentAbsent = students.Count - studentPresent;

           
            var activeDevices = logs
                .Select(l => l.DeviceId)
                .Distinct()
                .Count();

            
            var courseCount = await _instituteRepository.GetCourseCountByInstituteId(instituteId);

            return new InstituteDashboardDto
            {
                InstituteName = instituteName ?? string.Empty,

                TotalFaculty = faculty.Count,
                FacultyPresent = facultyPresent,
                FacultyAbsent = facultyAbsent,

                TotalStudents = students.Count,
                StudentsPresent = studentPresent,
                StudentsAbsent = studentAbsent,

                TotalCourses = courseCount,
                //ActiveDevices = activeDevices,
                //InactiveDevices = deviceIds.Count - activeDevices
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
            var UtcActual = DateTime.UtcNow;
            var todayUtc = DateTime.UtcNow
                .AddHours(PakistanUtcOffset)
                .Date
                .AddHours(-PakistanUtcOffset);

            var tomorrowUtc = UtcActual.AddDays(1);

           
            var todayLogs = await _instituteRepository
                .GetTimeLogs(deviceIds, UtcActual, tomorrowUtc);

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

        public async Task<InstituteAttendanceReportDto> GetAttendanceReportAsync(int instituteId, DateTime ?startDate, DateTime? endDate)
        {

            const int PakistanUtcOffset = 5;
            var todayPk = DateTime.UtcNow.AddHours(PakistanUtcOffset).Date;
            var startPk = (startDate ?? todayPk).Date;
            var endPk = (endDate ?? todayPk).Date.AddDays(1);
            var startUtc = startPk.AddHours(-PakistanUtcOffset);
            var endUtc = endPk.AddHours(-PakistanUtcOffset);
            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);

            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            if (!deviceIds.Any())
                return null;

            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

            var logs = await _instituteRepository
                     .GetTimeLogs(deviceIds, startUtc, endUtc);
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
