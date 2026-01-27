using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
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
        public async Task<CourseDto> AddCourseAsync(CourseDto dto)


        {
            var newCourse = new Course
            {
                CourseName = dto.CourseName,
                CourseCode = dto.CourseCode,
                Duration = dto.Duration,
                IsDeleted = false,
                InstituteId = dto.InstituteId,
                CreatedAt=DateTime.UtcNow,

            };
            await _courseRepository.CreateCourse(newCourse);
            return dto;

        }


        public async Task<bool> DeleteCourseAsync(int courseId)
        {
            var course = await _courseRepository.DeleteCourse(courseId);
            return course;
        }

        public async Task<CourseDto> UpdateCourseAsync( CourseDto dto)
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
