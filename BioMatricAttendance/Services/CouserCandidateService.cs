using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Response;

namespace BioMatricAttendance.Services
{
    public class CouserCandidateService:ICourseCandidateService
    {
        private readonly ICourseCandidatesRepository _courseCandidateRepo;
        public CouserCandidateService(ICourseCandidatesRepository courseCandidateRepo) {
        _courseCandidateRepo = courseCandidateRepo;
        }

        public async Task<CourseCandidateResponse> GetCouseInfoAsync(int courseId)
        {
          return  await _courseCandidateRepo.GetCouseInfoAsync(courseId);

        }
        public async Task AddCourseCandidates(CourseCandidateDto dto)
        {
             await _courseCandidateRepo.AddCourseCandidates(dto);
        }
        public async Task RemoveCourseCandidates(CandidateCourses entity)
        {
            await _courseCandidateRepo.RemoveCourseCandidates(entity);

        }
        public async Task<CandidateCourses?> GetById(int id)
        {
           return await _courseCandidateRepo.GetById(id); 
        }
    }
}
