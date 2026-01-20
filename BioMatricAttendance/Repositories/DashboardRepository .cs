using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BioMatricAttendance.Repositories
{
    public class DashboardRepository: IDashboardRepository
    {
        private readonly AppDbContext _context;

        public DashboardRepository(AppDbContext context)
        {
            _context = context;
        }
        //date base count.
        //set get counts by region id.
        //institute wise overview.intstitute Name,Region,Faculty Present,Faculty Absent,Student Present,Student Absent,Devices Active,Devices Inactive,Registered Courses today present/total students.by region id
        //report generation.


        //public async Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int? regionId)
        //{
        //    var today = DateTime.UtcNow.Date;

        //    // Institutes filtered by region (or all)
        //    var instituteQuery = _context.Institutes
        //        .Where(i => !i.IsDeleted && regionId.HasValue);

        //    var instituteIds = await instituteQuery
        //        .ToListAsync();



        //    // Counts
        //    var totalInstitutes = instituteIds.Count;

        //    //institute=>getdevices=>Candidate
        //    List<BiomatricDevice> devices = await _context.BiomatricDevices
        //       .Where(d => d.InstituteId == instituteId)
        //       .ToListAsync();


        //    var facultyPresent = await _context.Candidates
        //        .CountAsync(f =>
        //            f.AttendanceDate == today &&
        //            f.Status == "Present" &&
        //            instituteIds.Contains(f.InstituteId));

        //    //var facultyAbsent = await _context.FaculityAttendances
        //    //    .CountAsync(f =>
        //    //        f.AttendanceDate == today &&
        //    //        f.Status == "Absent" &&
        //    //        instituteIds.Contains(f.InstituteId));

        //    //var studentPresent = await _context.StudentAttendances
        //    //    .CountAsync(s =>
        //    //        s.AttendanceDate == today &&
        //    //        s.Status == "Present" &&
        //    //        instituteIds.Contains(s.InstituteId));

        //    //var studentAbsent = await _context.StudentAttendances
        //    //    .CountAsync(s =>
        //    //        s.AttendanceDate == today &&
        //    //        s.Status == "Absent" &&
        //    //        instituteIds.Contains(s.InstituteId));

        //    //var totalDevicesActive = await _context.BiomatricDevices
        //    //    .CountAsync(d =>
        //    //        d.Status == "Active" &&
        //    //        instituteIds.Contains(d.InstituteId));

        //    //var totalDevicesInactive = await _context.BiomatricDevices
        //    //    .CountAsync(d =>
        //    //        d.Status == "Inactive" &&
        //    //        instituteIds.Contains(d.InstituteId));

        //    //var totalRegisteredCourses = await _context.Courses
        //    //    .CountAsync(c =>
        //    //        !c.IsDeleted &&
        //    //        instituteIds.Contains(c.InstituteId));

        //    //// Percentages
        //    //var totalFaculty = facultyPresent + facultyAbsent;
        //    //var facultyPresentPercentage =
        //    //    totalFaculty == 0 ? 0 : (facultyPresent * 100) / totalFaculty;

        //    //var facultyAbsentPercentage =
        //    //    totalFaculty == 0 ? 0 : (facultyAbsent * 100) / totalFaculty;

        //    //var totalStudents = studentPresent + studentAbsent;
        //    //var studentsPresentPercentage =
        //    //    totalStudents == 0 ? 0 : (studentPresent * 100) / totalStudents;

        //    //var studentsAbsentPercentage =
        //    //    totalStudents == 0 ? 0 : (studentAbsent * 100) / totalStudents;

        //    return new SuperAdminDashboardDto
        //    {
        //        TotalInstitutes = totalInstitutes,
        //        TotalFacultyPresent = facultyPresent,
        //        TotalFacultyAbsent = facultyAbsent,
        //        TotalStudentsPresent = studentPresent,
        //        TotalStudentsAbsent = studentAbsent,
        //        TotalDevicesActive = totalDevicesActive,
        //        TotalDevicesInactive = totalDevicesInactive,
        //        TotalRegisteredCourses = totalRegisteredCourses,
        //        FacultyPresentPercentage = facultyPresentPercentage,
        //        FacultyAbsentPercentage = facultyAbsentPercentage,
        //        StudentsPresentPercentage = studentsPresentPercentage,
        //        StudentsAbsentPercentage = studentsAbsentPercentage
        //    };
        //}
        public async Task<SuperAdminDashboardDto> GetSuperAdminDashboardAsync(int? regionId)
        {
            var today = DateTime.UtcNow.Date;
            var institutesQuery = _context.Institutes
                .Include(i => i.Region)
                .Include(i => i.BiomatricDevices)
                .Include(i => i.Courses)
                .Where(i => !i.IsDeleted);

            if (regionId.HasValue && regionId.Value > 0)
            {
                institutesQuery = institutesQuery.Where(i => i.RegionId == regionId.Value);
            }

            var institutes = await institutesQuery.ToListAsync();
            var instituteIds = institutes.Select(i => i.Id).ToList();

            var devices = await _context.BiomatricDevices
                .Where(d => instituteIds.Contains(d.InstituteId) && !d.IsDeleted)
                .ToListAsync();

            var deviceIds = devices.Select(d => d.DeviceId).ToList();

           
            var allCandidates = await _context.Candidates
                .Where(c => deviceIds.Contains(c.DeviceId) && c.Enable)
                .ToListAsync();

            var faculty = allCandidates.Where(c => c.Previliges == "Manager" ).ToList();
            var students = allCandidates.Where(c => c.Previliges == "NormalUser").ToList();

          
            
            var todayStart = DateTime.UtcNow.AddHours(-5);
            var todayEnd = today.AddDays(1);
            var todayLogs = await _context.TimeLogs
       .Where(t => deviceIds.Contains(t.DeviceId) &&
                   t.PunchTime >= todayStart &&
                     t.PunchTime < todayStart.AddDays(1) &&
                   t.DeviceUserId > 0)
       .ToListAsync();
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

                var instituteCandidates = allCandidates.Where(c => instituteDeviceIds.Contains(c.DeviceId)).ToList();
                var instituteFaculty = instituteCandidates.Where(c => c.Previliges == "Faculty" || c.Previliges == "Teacher").ToList();
                var instituteStudents = instituteCandidates.Where(c => c.Previliges == "Student").ToList();

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
        //public async Task<List<InstituteDashboardRowDto>> GetInstituteTableAsync(int? regionId)
        //{
        //    var query = _context.Institutes
        //        .Where(i => !i.IsDeleted)
        //        .AsQueryable();


        //    if (regionId.HasValue)
        //    {
        //        query = query.Where(i => i.RegionId == regionId.Value);
        //    }

        //    return await query
        //        .Select(i => new InstituteDashboardRowDto
        //        {
        //            InstituteName = i.InstituteName,
        //            RegionName = i.Region.RegionName,

        //            FacultyPresent = _context.FaculityAttendances
        //                .Count(f => f.InstituteId == i.Id && f.Status == "Present"),

        //            FacultyAbsent = _context.FaculityAttendances
        //                .Count(f => f.InstituteId == i.Id && f.Status == "Absent"),

        //            StudentPresent = _context.StudentAttendances
        //                .Count(s => s.InstituteId == i.Id && s.Status == "Present"),

        //            StudentAbsent = _context.StudentAttendances
        //                .Count(s => s.InstituteId == i.Id && s.Status == "Absent"),

        //            DevicesActive = _context.BiomatricDevices
        //                .Count(d => d.InstituteId == i.Id && d.Status == "Active"),

        //            DevicesInactive = _context.BiomatricDevices
        //                .Count(d => d.InstituteId == i.Id && d.Status == "Inactive"),

        //            RegisteredCourses = _context.Courses
        //                .Count(c => c.InstituteId == i.Id && !c.IsDeleted)
        //        })
        //        .ToListAsync();
        //}





    }
}
