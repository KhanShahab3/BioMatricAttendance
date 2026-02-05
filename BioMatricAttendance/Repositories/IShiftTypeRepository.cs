using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;

namespace BioMatricAttendance.Repositories
{
    public interface IShiftTypeRepository
    {
        Task<ShiftType> GetShiftById(int Id);
        Task<ShiftType> CreateShift(ShiftType shift);
        Task<List<ShiftType>> GetAllShifts();
        Task<bool> DeleteShiftType(int Id);
        Task<ShiftType> UpdateShiftType(ShiftType shift);

    }
}
