using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Helper;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{
 
    public class InstituteDashboardService : IInstituteDashboardService
    {
        private readonly IInstituteAttendanceRepository _instituteRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IInstituteRepository _insRepo;
        private readonly ICourseCandidatesRepository _courseCandidatesRepository;
        private readonly AppDbContext _appDbContext;
        public InstituteDashboardService(IInstituteAttendanceRepository instituteRepository, 
            ICourseRepository courseRepository,
            IInstituteRepository insRepo,
            ICourseCandidatesRepository courseCandidatesRepository,
            AppDbContext appDbContext)
        {
            _instituteRepository = instituteRepository;
            _courseRepository = courseRepository;
            _insRepo = insRepo;
            _courseCandidatesRepository = courseCandidatesRepository;
            _appDbContext = appDbContext;
        }

        public async Task<InstituteDashboardDto> GetInstituteDashboard(int? instituteId)
        {
            //var today = DateTime.UtcNow.AddHours(5).Date;
            //var fromUtc = today.AddHours(-5);
            //var toUtc = fromUtc.AddDays(1);



         

            // Pakistan today
      


            var(startUtc, endUtc)=DateTimeHelper.GetUtcRangeForPakistanDate(null,null);


            var today = startUtc.Date;




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
            var faculityMaleCount = faculty.Where(f => f.gender == Gender.Male).Count();
            var faculityFemaleCount = faculty.Where(f => f.gender == Gender.Female).Count();
            var StudentMaleCount = students.Where(f => f.gender == Gender.Male).Count();
            var StudentFemaleCount = students.Where(f => f.gender == Gender.Female).Count();
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


            var todayShiftAssignments = await _appDbContext.CandidateShifts
      .Include(cs => cs.Candidate)
      .Where(cs =>
          cs.ShiftDate.Date == today &&
          deviceIds.Contains(cs.Candidate.DeviceId))
      .ToListAsync();

            var shiftIds = todayShiftAssignments
                .Select(cs => cs.ShiftId)
                .Distinct()
                .ToList();

            var shifts = await _appDbContext.ShiftTypes
                .Where(s => shiftIds.Contains(s.Id) && !s.isDeleted)
                .ToListAsync();

            var shiftWiseAttendance = shifts.Select(shift =>
            {
                var shiftCandidates = todayShiftAssignments
                    .Where(cs => cs.ShiftId == shift.Id)
                    .Select(cs => cs.Candidate)
                    .ToList();

                var shiftStudents = shiftCandidates
                    .Where(c => c.Previliges == "NormalUser")
                    .ToList();

                var shiftFaculty = shiftCandidates
                    .Where(c => c.Previliges == "Manager")
                    .ToList();

                var shiftStudentPresent =
                    shiftStudents.Count(c => presentUserIds.Contains(c.DeviceUserId));

                var shiftFacultyPresent =
                    shiftFaculty.Count(c => presentUserIds.Contains(c.DeviceUserId));

                return new ShiftWiseAttendanceDto
                {
                    ShiftId = shift.Id,
                    ShiftName = shift.ShiftName,

                    StudentPresent = shiftStudentPresent,
                    StudentAbsent = shiftStudents.Count - shiftStudentPresent,

                    FacultyPresent = shiftFacultyPresent,
                    FacultyAbsent = shiftFaculty.Count - shiftFacultyPresent
                };
            }).ToList();


            var courseCount = await _instituteRepository.GetCourseCountByInstituteId(instituteId);

            return new InstituteDashboardDto
            {
                InstituteName = instituteName ?? string.Empty,

                TotalFaculty = faculty.Count,
                FacultyPresent = facultyPresent,
                FacultyAbsent = facultyAbsent,
                FacultyFemale=faculityFemaleCount,
                FacultyMale=faculityMaleCount,
                StudentFemale=StudentFemaleCount,
                StudentMale=StudentMaleCount,
                TotalStudents = students.Count,
                StudentsPresent = studentPresent,
                StudentsAbsent = studentAbsent,

                TotalCourses = courseCount,
                ShiftWiseAttendance = shiftWiseAttendance
                //ActiveDevices = activeDevices,
                //InactiveDevices = deviceIds.Count - activeDevices
            };
        }



        public async Task<List<CourseAttendanceDto>> GetCourseWiseAttendanceAsync(int? instituteId)
        {

            var(toayUtc, tomorrowUtc)=DateTimeHelper.GetUtcRangeForPakistanDate(null,null);

            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);

            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            if (!deviceIds.Any())
                return new List<CourseAttendanceDto>();

           
            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);

            
            var students = candidates
                .Where(c => c.Previliges == "NormalUser")
                .ToList();



           
            var todayLogs = await _instituteRepository
                .GetTimeLogs(deviceIds, toayUtc, tomorrowUtc);

            var presentStudentIds = todayLogs
                .Select(t => (int)t.DeviceUserId)
                .Distinct()
                .ToHashSet();

           
            var courses = await _courseRepository.GetCourseByInstituteId(instituteId);


          
            var result = new List<CourseAttendanceDto>();

            foreach (var course in courses)
            {
                var coursestudentinfo = await _courseCandidatesRepository.GetCandidateByCourseId(course.Id);

                var studentIds = coursestudentinfo
     .Select(l => (int)l.CandidateId)
     .Distinct()
     .ToHashSet();

                var courseStudents = students
                    .Where(s => studentIds.Contains(s.Id) )
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

        public async Task<InstituteAttendanceReportDto> GetAttendanceReportAsync(int? instituteId, DateTime ?startDate, DateTime? endDate)
        {

          var(startUtc, endUtc) = DateTimeHelper.GetUtcRangeForPakistanDate(startDate, endDate);    
            var devices = await _instituteRepository.GetDevicesByInstituteId(instituteId);

            var deviceIds = devices.Select(d => d.DeviceId).ToList();

            if (!deviceIds.Any())
                return null;

            var candidates = await _instituteRepository.GetCandidatesByDeviceIds(deviceIds);

            var faculty = candidates.Where(c => c.Previliges == "Manager").ToList();
            var students = candidates.Where(c => c.Previliges == "NormalUser").ToList();

            var logs = await _instituteRepository
                     .GetTimeLogs(deviceIds, startUtc, endUtc);
            var presentUserIds = logs
                .Select(l => (int)l.DeviceUserId).Distinct().ToHashSet();

            
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
                var coursestudentinfo = await _courseCandidatesRepository.GetCandidateByCourseId(course.Id);

                var studentIds = coursestudentinfo
              .Select(l => (int)l.CandidateId)
              .Distinct()
              .ToHashSet();

                var courseStudents = students
                    .Where(s => studentIds.Contains(s.Id))
                    .ToList();
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
