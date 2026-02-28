using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Repositories;
using BioMatricAttendance.Response;
using Microsoft.EntityFrameworkCore;
using System;
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
         
            var instituteQuery = _appDbContext.Institutes
                .Where(i => !i.IsDeleted);

            if (instituteId.HasValue && instituteId.Value > 0)
                instituteQuery = instituteQuery.Where(i => i.Id == instituteId.Value);

            if (regionId.HasValue && regionId.Value > 0)
                instituteQuery = instituteQuery.Where(i => i.RegionId == regionId.Value);

        
            var deviceIds = await instituteQuery
                .SelectMany(i => i.BiomatricDevices
                    .Where(d => d.isRegistered)
                    .Select(d => d.DeviceId))
                .Distinct()
                .ToListAsync();

            if (!deviceIds.Any())
                return new List<CandidateWithShiftDto>();

      
            var candidates = await _appDbContext.Candidates
                .Where(c => c.Enable && deviceIds.Contains(c.DeviceId))
                .ToListAsync();

            if (!candidates.Any())
                return new List<CandidateWithShiftDto>();

            var candidateIds = candidates.Select(c => c.Id).ToList();

           
            var candidateShifts = await _appDbContext.CandidateShifts
                .Include(cs => cs.Shift)
                .Where(cs => candidateIds.Contains(cs.CandidateId))
                .ToListAsync();

            var latestShiftByCandidate = candidateShifts
                .GroupBy(cs => cs.CandidateId)
                .Select(g => g.OrderByDescending(cs => cs.CreatedAt).First())
                .ToDictionary(cs => cs.CandidateId);

          
            var result = candidates.Select(c =>
            {
                latestShiftByCandidate.TryGetValue(c.Id, out var assignedShift);
                return new CandidateWithShiftDto
                {
                    CandidateId = c.Id,
                    Name = c.Name,
                    ShiftId = assignedShift?.ShiftId,
                    ShiftName = assignedShift?.Shift?.ShiftName,
                    IsAssigned = assignedShift != null
                };
            }).ToList();

            return result;
        }







        public async Task<APIResponse<string>> AssignShiftAsync(AssignShiftDto dto)
        {
            // Candidate Validation
            var validCandidateIds = await _appDbContext.Candidates
                .Where(c => dto.CandidateIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            var invalidIds = dto.CandidateIds.Except(validCandidateIds).ToList();
            if (invalidIds.Any())
            {
                return new APIResponse<string>
                {
                    Sucess = false,
                    Message = $"Invalid Candidate Id(s): {string.Join(",", invalidIds)}",
                    StatusCode = 400,
                    Data = null
                };
            }

            var selectedIds = validCandidateIds.ToHashSet();

            if (dto.ShiftId == null || dto.ShiftId <= 0)
            {
                return new APIResponse<string>
                {
                    Sucess = false,
                    Message = "Shift type is required for assignment",
                    StatusCode = 400,
                    Data = null
                };
            }


            var existingShifts = await _appDbContext.CandidateShifts
                .Where(cs => selectedIds.Contains(cs.CandidateId))
                .ToListAsync();


            if (existingShifts==null)
            {
                return new APIResponse<string>
                {
                    Sucess = false,
                    Message = $"Shift with Id {dto.ShiftId} does not exist",
                    StatusCode = 400,
                    Data = null
                };
            }
            _appDbContext.CandidateShifts.RemoveRange(existingShifts);
            int deleteCount = existingShifts.Count;

            if (dto.ShiftId > 0)  
            {
              
                var shiftExists = await _appDbContext.ShiftTypes
                    .AnyAsync(s => s.Id == dto.ShiftId);

                if (!shiftExists)
                {
                    return new APIResponse<string>
                    {
                        Sucess = false,
                        Message = $"Shift with Id {dto.ShiftId} does not exist",
                        StatusCode = 400,
                        Data = null
                    };
                }

                var newShifts = selectedIds.Select(id => new CandidateShift
                {
                    CandidateId = id,
                    ShiftId = dto.ShiftId,
                    CreatedAt = DateTime.UtcNow
                });

                await _appDbContext.CandidateShifts.AddRangeAsync(newShifts);
            }

            await _appDbContext.SaveChangesAsync();

           
            string message;
            if (deleteCount > 0 && dto.ShiftId > 0)
                message = "Shift update";
            else if (deleteCount == 0 && dto.ShiftId > 0)
                message = "Shift assign";
            else
                message = "No changes made";

            return new APIResponse<string>
            {
                Sucess = true,
                Message = message,
                StatusCode = 200,
                Data = null
            };
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

        public async Task<APIResponse<string>> RemoveShiftAsync(int candidateId)
        {
            var shiftToRemove = await _appDbContext.CandidateShifts
      .FirstOrDefaultAsync(cs => cs.CandidateId == candidateId);

            if (shiftToRemove != null)
            {
                _appDbContext.CandidateShifts.Remove(shiftToRemove);
                await _appDbContext.SaveChangesAsync();

                return new APIResponse<string>
                {
                    Sucess = true,
                    Message = $"Candidate {candidateId} removed",
                    StatusCode = 200,
                    Data = null
                };
            }

            return new APIResponse<string>
            {
                Sucess = true,
                Message = $"Candidate not found",
                StatusCode = 200,
                Data = null
            };

        }






    }
}
