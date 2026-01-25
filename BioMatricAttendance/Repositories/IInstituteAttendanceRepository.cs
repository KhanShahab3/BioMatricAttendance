using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IInstituteAttendanceRepository
    {
        //Task<Institute> GetInstituteById(int instituteId);

    
        Task<List<BiomatricDevice>> GetDevicesByInstituteId(int instituteId);

        Task<List<Candidate>> GetCandidatesByDeviceIds(List<long> deviceIds);

       
        Task<List<TimeLogs>> GetTimeLogs(List<long> deviceIds, DateTime from, DateTime to);

      
        Task<int> GetCourseCountByInstituteId(int instituteId);
    }
}
