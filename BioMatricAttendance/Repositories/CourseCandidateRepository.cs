using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class CourseCandidateRepository:ICourseCandidatesRepository
    {
        private readonly AppDbContext _context;
        public CourseCandidateRepository(AppDbContext context) {
            _context = context;
        }

        public async Task<List<CandidateCourses>> GetCandidateByCourseId(int courseId)
        {
            var courses = await _context.CandidateCourses
                .Where(c => c.CourseId == courseId && !c.IsDeleted).ToListAsync();
            return courses;
        }

        public async Task<CourseCandidateResponse> GetCouseInfoAsync(int courseId)
        {
            
            var courseCandidates = await _context.Courses
            .Where(c => c.Id == courseId)
            .Select(c => new CourseCandidateResponse
            {
                CourseName = c.CourseName,
                Candaitesinfos = c.CandidateCourses
                    .Select(cc => new CourseCandidatesinfoResponse
                    {
                        CourseCandidateId = cc.Id,
                        CandidateName = cc.Candidate.Name,
                        CandidateId=cc.CandidateId

                    })
                    .ToList()
            }).FirstOrDefaultAsync();


            return courseCandidates;
        }
        public async Task AddCourseCandidates(CourseCandidateDto dto)
        {
            var candidateCourses = dto.StudentIds
                .Select(studentId => new CandidateCourses
                {
                    CourseId = dto.CourseId,
                    CandidateId = studentId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                })
                .ToList();

            await _context.CandidateCourses.AddRangeAsync(candidateCourses);
            await _context.SaveChangesAsync();
        }
        public async Task RemoveCourseCandidates(CandidateCourses entity)
        {
             _context.CandidateCourses.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<CandidateCourses?> GetById(int id)
        {
            return await _context.CandidateCourses.Where(x => x.Id == id).FirstOrDefaultAsync();

        }
    }
}
