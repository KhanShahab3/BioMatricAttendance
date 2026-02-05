using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface ILeaveManagmentService
    {
        Task<List<AbsentCandidateDto>> GetAbsentCandidates(int? regionId, int? instituteId, DateTime startDate, DateTime endDate);

        Task AssignLeave(AssignLeaveDto assignLeaveDto);
    }
}
