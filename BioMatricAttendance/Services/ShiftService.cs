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

        public async Task<List<CandidateWithShiftDto>> GetCandidatesWithShift(
            int? instituteId,
            int? regionId
            )
        {
         
            var deviceIdsQuery = _appDbContext.Institutes
                .Where(i => !i.IsDeleted);

            if ( instituteId > 0)
                deviceIdsQuery = deviceIdsQuery.Where(i => i.Id == instituteId);

            if ( regionId > 0)
                deviceIdsQuery = deviceIdsQuery.Where(i => i.RegionId == regionId);

            var deviceIds = await deviceIdsQuery
                .SelectMany(i => i.BiomatricDevices.Where(d => d.isRegistered).Select(d => d.DeviceId))
                .ToListAsync();

            if (!deviceIds.Any())
                return new List<CandidateWithShiftDto>();

           
            var candidates = await _appDbContext.Candidates
                .Where(c => c.Enable && deviceIds.Contains(c.DeviceId))
                .ToListAsync();

           
            var candidateShiftsQuery = _appDbContext.CandidateShifts
                .Include(cs => cs.Shift)
                .Where(cs => deviceIds.Contains(cs.Candidate.DeviceId));

            //if (shiftId > 0)
            //{
            //    candidateShiftsQuery = candidateShiftsQuery.Where(cs => cs.ShiftId == shiftId); 
            //    var candidateIdsWithShift = candidateShiftsQuery.Select(cs => cs.CandidateId).ToHashSet();
            //    candidates = candidates.Where(c => candidateIdsWithShift.Contains(c.Id)).ToList();
            //}


            var result = candidates.Select(c =>
            {
                var assignedShift = candidateShiftsQuery
       .FirstOrDefault(cs => cs.CandidateId == c.Id
                             );

                var shiftName = assignedShift?.Shift?.ShiftName;

                return new CandidateWithShiftDto
                {
                    CandidateId = c.Id,
                    Name = c.Name,
                    ShiftId = assignedShift?.ShiftId,
                    ShiftName = shiftName,
                    IsAssigned = assignedShift != null
                };
            }).ToList();

            return result;
        }







        public async Task AssignShiftAsync(AssignShiftDto dto)
        {
            
            var existingShifts = await _appDbContext.CandidateShifts
                 .Where(cs => cs.ShiftId == dto.ShiftId)
                .ToListAsync();

            if (existingShifts.Any())
            {
               
                _appDbContext.CandidateShifts.RemoveRange(existingShifts);
            }

            
            var assignments = dto.CandidateIds.Select(id => new CandidateShift
            {
                CandidateId = id,
                ShiftId = dto.ShiftId,
                CreatedAt = DateTime.UtcNow
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
