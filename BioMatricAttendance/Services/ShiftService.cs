using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BioMatricAttendance.Services
{
    public class ShiftService : IShiftService

    {
        private readonly AppDbContext _appDbContext;
        private readonly IShiftTypeRepository _shift;
        public ShiftService(AppDbContext appDbContext, IShiftTypeRepository shift)
        {
            _appDbContext = appDbContext;
            _shift = shift;
        }




        public async Task AssignShiftAsync(AssignShiftDto dto)
        {
          
            var existingShifts = await _appDbContext.CandidateShifts
                .Where(cs => dto.CandidateIds.Contains(cs.CandidateId)
                           )
                .ToListAsync();

            if (existingShifts.Any())
            {
                _appDbContext.CandidateShifts.RemoveRange(existingShifts);
            }

     
            var assignments = dto.CandidateIds.Select(id => new CandidateShift
            {
                CandidateId = id,
                ShiftId = dto.ShiftId,
                ShiftDate = dto.ShiftDate.Date
            }).ToList();

            await _appDbContext.CandidateShifts.AddRangeAsync(assignments);
            await _appDbContext.SaveChangesAsync();
        }



        public async Task<List<ShiftType>> GetAllShifts()
        {
            var shiftTypes = await _shift.GetAllShifts();
            return shiftTypes;


        }

        public async Task<ShiftType> GetShiftById(int Id)
        {
            return await _shift.GetShiftById(Id);

        }
        public async Task<ShiftType> CreateShift(ShiftType shift)
        {
            return await _shift.CreateShift(shift);
        }

        public async Task<ShiftType> UpdateShiftType(ShiftType shiftType)
        {
            return await _shift.UpdateShiftType(shiftType);
        }

        public async Task<bool> DeleteShiftType(int id)
        {
            return await _shift.DeleteShiftType(id);

        }

      
     


    }
}
