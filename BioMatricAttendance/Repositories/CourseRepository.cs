using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class CourseRepository:ICourseRepository
    {
        private readonly AppDbContext _appDbContext;
        public CourseRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<Course>> GetCourseByInstituteId(int instituteId)
        {
            var courses=await _appDbContext.Courses
                .Where(c=>c.InstituteId==instituteId && !c.IsDeleted)
                .ToListAsync();
                return courses;
        }
    }
}
