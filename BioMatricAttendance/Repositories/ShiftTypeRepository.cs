using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Repositories
{
    public class ShiftTypeRepository:IShiftTypeRepository
    {
        private readonly AppDbContext _appDbContext;
        public ShiftTypeRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<ShiftType>> GetAllShifts()
        {
            return await _appDbContext.ShiftTypes
                .Where(st => !st.isDeleted)
                .ToListAsync();
        }
        public async Task<ShiftType> GetShiftById(int Id)
        {
            return await _appDbContext.ShiftTypes
                .FirstOrDefaultAsync(st => st.Id == Id && !st.isDeleted);
        }

        public async Task<ShiftType> CreateShift(ShiftType shift)
        {
            _appDbContext.ShiftTypes.Add(shift);
            await _appDbContext.SaveChangesAsync();
            return shift;
        }

        public async Task<ShiftType> UpdateShiftType(ShiftType shift)
        {
            var existing = await _appDbContext.ShiftTypes
                .FirstOrDefaultAsync(st => st.Id == shift.Id && !st.isDeleted);
            if (existing == null) return null;
            existing.ShiftName = shift.ShiftName;
            existing.StartTime = shift.StartTime;
            existing.EndTime = shift.EndTime;
            await _appDbContext.SaveChangesAsync();
            return existing;
        }
        public async Task<bool> DeleteShiftType(int Id)
        {
            var existing = await _appDbContext.ShiftTypes
                .FirstOrDefaultAsync(st => st.Id == Id && !st.isDeleted);
            if (existing == null) return false;
            existing.isDeleted = true;
            await _appDbContext.SaveChangesAsync();
            return true;

        }
    }
}
