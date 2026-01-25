using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Repositories;

namespace BioMatricAttendance.Services
{
    public class CourseService:ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        public CourseService(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<GetCourseDto>> GetCoursesByInstituteId(int instituteId)
        {
            var courses=await _courseRepository.GetCourseByInstituteId(instituteId);
            var courseDto=courses.Select(c=>new GetCourseDto
            {
                CourseCode = c.CourseCode,
                CourseName = c.CourseName,
                Duration = c.Duration,
            })
                .ToList();
            return courseDto;
        }
    }
}
