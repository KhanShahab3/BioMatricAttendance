using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Services
{
    public interface IShiftService
    {
        Task AssignShiftAsync(AssignShiftDto dto);

        Task<ShiftType> GetShiftById(int Id);
        Task<ShiftType> CreateShift(ShiftType shift);
        Task<List<ShiftType>> GetAllShifts();
        Task<bool> DeleteShiftType(int Id);
        Task<ShiftType> UpdateShiftType(ShiftType shift);
        Task<List<CandidateWithShiftDto>> GetCandidatesWithShift(
             int? instituteId ,
             int? regionId,
             int? shiftId);
    }
}
