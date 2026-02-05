using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace BioMatricAttendance.Services
{
    public class ShiftService:IShiftService

    {
        private readonly AppDbContext _appDbContext;
        public ShiftService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }




        public async Task AssignShiftAsync(AssignShiftDto dto)
        {
           
            var existing = await _appDbContext.CandidateShifts
                .Where(cs => dto.CandidateIds.Contains(cs.CandidateId)
                             && cs.ShiftDate.Date == dto.ShiftDate.Date)
                .Select(cs => cs.CandidateId)
                .ToListAsync();

            var toAssign = dto.CandidateIds.Except(existing).ToList();

            if (!toAssign.Any()) return;

            var assignments = toAssign.Select(id => new CandidateShift
            {
                CandidateId = id,
                ShiftId = dto.ShiftId,
                ShiftDate = dto.ShiftDate.Date,
                //AssignedBy = dto.AssignedBy
            }).ToList();

            await _appDbContext.CandidateShifts.AddRangeAsync(assignments);
            await _appDbContext.SaveChangesAsync();
        }

    }
}
