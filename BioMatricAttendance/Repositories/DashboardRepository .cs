using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class DashboardRepository
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


        //public async Task<SuperAdminDashboardDto>GetSuperAdminDashboardAsync(int? regionId)
        //{
        //    var today = DateTime.UtcNow.Date;

        //    // Institutes filtered by region (or all)
        //    var instituteQuery = _context.Institutes
        //        .Where(i =>!i.IsDeleted && regionId.HasValue);

        //    var instituteIds = await instituteQuery
        //        .Select(i => i.Id)
        //        .ToListAsync();

        //    // Counts
        //    var totalInstitutes = instituteIds.Count;

        //    var facultyPresent = await _context.FaculityAttendances
        //        .CountAsync(f =>
        //            f.AttendanceDate == today &&
        //            f.Status == "Present" &&
        //            instituteIds.Contains(f.InstituteId));

        //    var facultyAbsent = await _context.FaculityAttendances
        //        .CountAsync(f =>
        //            f.AttendanceDate == today &&
        //            f.Status == "Absent" &&
        //            instituteIds.Contains(f.InstituteId));

        //    var studentPresent = await _context.StudentAttendances
        //        .CountAsync(s =>
        //            s.AttendanceDate == today &&
        //            s.Status == "Present" &&
        //            instituteIds.Contains(s.InstituteId));

        //    var studentAbsent = await _context.StudentAttendances
        //        .CountAsync(s =>
        //            s.AttendanceDate == today &&
        //            s.Status == "Absent" &&
        //            instituteIds.Contains(s.InstituteId));

        //    var totalDevicesActive = await _context.BiomatricDevices
        //        .CountAsync(d =>
        //            d.Status == "Active" &&
        //            instituteIds.Contains(d.InstituteId));

        //    var totalDevicesInactive = await _context.BiomatricDevices
        //        .CountAsync(d =>
        //            d.Status == "Inactive" &&
        //            instituteIds.Contains(d.InstituteId));

        //    var totalRegisteredCourses = await _context.Courses
        //        .CountAsync(c =>
        //            !c.IsDeleted &&
        //            instituteIds.Contains(c.InstituteId));

        //    // Percentages
        //    var totalFaculty = facultyPresent + facultyAbsent;
        //    var facultyPresentPercentage =
        //        totalFaculty == 0 ? 0 : (facultyPresent * 100) / totalFaculty;

        //    var facultyAbsentPercentage =
        //        totalFaculty == 0 ? 0 : (facultyAbsent * 100) / totalFaculty;

        //    var totalStudents = studentPresent + studentAbsent;
        //    var studentsPresentPercentage =
        //        totalStudents == 0 ? 0 : (studentPresent * 100) / totalStudents;

        //    var studentsAbsentPercentage =
        //        totalStudents == 0 ? 0 : (studentAbsent * 100) / totalStudents;

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
