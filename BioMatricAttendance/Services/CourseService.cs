using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Response;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly AppDbContext _context;
        public CourseService(ICourseRepository courseRepository,AppDbContext context)
        {
            _courseRepository = courseRepository;
            _context = context;
        }

        public async Task<List<GetCourseDto>> GetCoursesByInstituteId(int instituteId)
        {
            var courses = await _courseRepository.GetCourseByInstituteId(instituteId);
            var courseDto = courses.Select(c => new GetCourseDto
            {
                Id=c.Id,
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Duration = c.Duration,
            })
                .ToList();
            return courseDto;
        }
        public async Task<CourseDto>AddCourseAsync(CourseDto dto)


        {
            try
            {
                var newCourse = new Course
                {
                    CourseName = dto.CourseName,
                    CourseCode = dto.CourseCode,
                    Duration = dto.Duration,
                    IsDeleted = false,
                    InstituteId = dto.InstituteId,
                    CreatedAt = DateTime.UtcNow,

                };
                await _courseRepository.CreateCourse(newCourse);
                return dto;
            }
            catch (Exception ex)
            {
                throw new Exception("DuplicateCode");
            }


        }


        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.DeleteCourse(courseId);
            return course;
        }

        public async Task<CourseDto> UpdateCourseAsync( CourseDto dto)
        {
            try
            {
                var updatedCourse = new Course
                {
                    Id = dto.Id,
                    CourseName = dto.CourseName,
                    CourseCode = dto.CourseCode,
                    Duration = dto.Duration,
                    IsDeleted = false,
                    InstituteId = dto.InstituteId,
                };
                await _courseRepository.UpdateCourse(updatedCourse);
                return dto;
            }
            catch (Exception ex)
            {

                throw new Exception("DuplicateCode");
            }
        }

        public async Task<List<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllCourses();
            var courseDtos = courses.Select(c => new CourseDto
            {
                Id = c.Id,
                CourseName = c.CourseName,
                CourseCode = c.CourseCode,
                Duration = c.Duration,
                InstituteId = c.InstituteId,
            }).ToList();
            return courseDtos;
        }
    }
}
