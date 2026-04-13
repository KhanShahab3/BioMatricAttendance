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


        public async Task<List<CandidateWithShiftDto>> GetCandidatesWithShift(int? instituteId, int? regionId)
        {
          
            var deviceIds = await _appDbContext.Institutes
                .Where(i => !i.IsDeleted &&
                           (!instituteId.HasValue || i.Id == instituteId) &&
                           (!regionId.HasValue || i.RegionId == regionId))
                .SelectMany(i => i.BiomatricDevices.Where(d => d.isRegistered).Select(d => d.DeviceId))
                .Distinct()
                .ToListAsync();

            if (!deviceIds.Any()) return new List<CandidateWithShiftDto>();

            
            var result = await _appDbContext.Candidates
                .Where(c => c.Enable && deviceIds.Contains(c.DeviceId))
                .Select(c => new CandidateWithShiftDto
                {
                    CandidateId = c.Id,
                    Name = c.Name,
                    ShiftId = _appDbContext.CandidateShifts
                        .Where(cs => cs.CandidateId == c.Id)
                        .OrderByDescending(cs => cs.CreatedAt)
                        .Select(cs => (int?)cs.ShiftId)
                        .FirstOrDefault(),

                   
                    ShiftName = _appDbContext.CandidateShifts
                        .Where(cs => cs.CandidateId == c.Id)
                        .OrderByDescending(cs => cs.CreatedAt)
                        .Select(cs => cs.Shift.ShiftName) 
                        .FirstOrDefault(),

                    IsAssigned = _appDbContext.CandidateShifts.Any(cs => cs.CandidateId == c.Id)
                })
                .ToListAsync();

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


            if (existingShifts.Any())
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
           
            var shiftsToRemove = await _appDbContext.CandidateShifts
                .Where(cs => cs.CandidateId == candidateId)
                .ToListAsync();

            if (shiftsToRemove.Any())
            {
                
                _appDbContext.CandidateShifts.RemoveRange(shiftsToRemove);
                await _appDbContext.SaveChangesAsync();

                return new APIResponse<string>
                {
                    Sucess = true,
                    Message = $"Candidate {candidateId} shifts removed successfully",
                    StatusCode = 200
                };
            }

           
            return new APIResponse<string>
            {
                Sucess = false,
                Message = $"No assigned shift found for candidate {candidateId}",
                StatusCode = 404 
            };
        }







    }
}
