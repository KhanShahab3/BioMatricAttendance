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

        public async Task<Course> CreateCourse(Course course)
        {
            var newCourse=await _appDbContext.Courses.AddAsync(course);
            await _appDbContext.SaveChangesAsync();
            return newCourse.Entity;
        }
        public async Task<Course>UpdateCourse(Course course)
        {
            var existingCourse=await _appDbContext.Courses
                .FirstOrDefaultAsync(c=>c.Id==course.Id && !c.IsDeleted);
            if(existingCourse==null)
            {
                return null;
            }
            existingCourse.CourseName=course.CourseName;
            existingCourse.CourseCode=course.CourseCode;
            existingCourse.Duration=course.Duration;
          
            await _appDbContext.SaveChangesAsync();
            return existingCourse;
        }

        public async Task<bool>DeleteCourse(int courseId)
        {
            var existingCourse=await _appDbContext.Courses
                .FirstOrDefaultAsync(c=>c.Id==courseId && !c.IsDeleted);
            if(existingCourse==null)
            {
                return false;
            }
            existingCourse.IsDeleted=true;
            await _appDbContext.SaveChangesAsync();
            return true;
        }
        public async Task<List<Course>>GetAllCourses()
        {
            var courses=await _appDbContext.Courses
                .Where(c=>!c.IsDeleted)
                .ToListAsync();
            return courses;
        }
    }
}
