using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Response;

namespace BioMatricAttendance.Services
{
    public interface ILeaveManagmentService
    {
        Task<List<AbsentCandidateDto>> GetAbsentCandidates(int? regionId, int? instituteId);

        Task<APIResponse<string>> AssignLeave(AssignLeaveDto dto);
    }
}
