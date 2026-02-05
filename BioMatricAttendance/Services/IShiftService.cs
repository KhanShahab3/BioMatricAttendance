using BioMatricAttendance.DTOsModel;

namespace BioMatricAttendance.Services
{
    public interface IShiftService
    {
        Task AssignShiftAsync(AssignShiftDto dto);
    }
}
