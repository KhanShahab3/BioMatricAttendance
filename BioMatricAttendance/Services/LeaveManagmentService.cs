using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Helper;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{
    public class LeaveManagmentService : ILeaveManagmentService
    {
        private readonly AppDbContext _appDbContext;
        public LeaveManagmentService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<AbsentCandidateDto>> GetAbsentCandidates(int? regionId, int? instituteId)
        {
            var today = DateTime.UtcNow.Date;
            var (startUtc, endUtc) = DateTimeHelper.GetUtcRangeForPakistanDate(today, today);
            var leaveDate = DateOnly.FromDateTime(startUtc.Date);

           
            var deviceIdsQuery = _appDbContext.Institutes
                .Where(i => !i.IsDeleted &&
                           (!instituteId.HasValue || instituteId == 0 || i.Id == instituteId) &&
                           (!regionId.HasValue || regionId == 0 || i.RegionId == regionId))
                .SelectMany(i => i.BiomatricDevices.Where(d => d.isRegistered).Select(d => d.DeviceId))
                .Distinct();

           
            var presentDeviceUserIdsQuery = _appDbContext.TimeLogs
                .Where(tl => tl.PunchTime >= startUtc && tl.PunchTime < endUtc)
                .Select(tl => tl.DeviceUserId)
                .Distinct();

           
            var absentCandidates = await _appDbContext.Candidates
                .Where(c => c.Enable && deviceIdsQuery.Contains(c.DeviceId)) 
                .Where(c => !presentDeviceUserIdsQuery.Contains(c.DeviceUserId)) 
                .Select(c => new AbsentCandidateDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    DeviceId = c.DeviceId,
                    DeviceUserId = c.DeviceUserId,
                    gender = c.gender,

                   
                    IsOnLeave = _appDbContext.Leaves.Any(l => l.CandidateId == c.Id && l.LeaveDate == leaveDate),

                    LeaveTypeId = _appDbContext.Leaves
                        .Where(l => l.CandidateId == c.Id && l.LeaveDate == leaveDate)
                        .Select(l => (int?)l.LeaveTypeId)
                        .FirstOrDefault(),

                    LeaveTypeName = _appDbContext.Leaves
                        .Where(l => l.CandidateId == c.Id && l.LeaveDate == leaveDate)
                        .Select(l => l.LeaveType.TypeName) 
                        .FirstOrDefault()
                })
                .ToListAsync();

            return absentCandidates;
        }







        public async Task<APIResponse<string>> AssignLeave(AssignLeaveDto dto)
        {
            if (dto.LeaveTypeId == null || dto.LeaveTypeId <= 0)
            {
                return new APIResponse<string> { Sucess = false, Message = "Leave type is required", StatusCode = 400 };
            }

            if (dto.CandidateIds == null || !dto.CandidateIds.Any())
            {
                return new APIResponse<string> { Sucess = false, Message = "No candidates selected", StatusCode = 400 };
            }

            
            var uniqueIds = dto.CandidateIds.Distinct().ToList();

         
            var (startDate, _) = DateTimeHelper.GetUtcRangeForPakistanDate(dto.LeaveDate, null);
            var leaveDate = DateOnly.FromDateTime(startDate);

           
            var validCandidateCount = await _appDbContext.Candidates
                .CountAsync(c => uniqueIds.Contains(c.Id));

            if (validCandidateCount != uniqueIds.Count)
            {
                return new APIResponse<string> { Sucess = false, Message = "One or more Candidate IDs are invalid", StatusCode = 400 };
            }

            var leaveTypeExists = await _appDbContext.LeaveTypes.AnyAsync(lt => lt.Id == dto.LeaveTypeId);
            if (!leaveTypeExists)
            {
                return new APIResponse<string> { Sucess = false, Message = "Invalid leave type", StatusCode = 400 };
            }

            
            var existingLeaves = await _appDbContext.Leaves
                .Where(l => l.LeaveDate == leaveDate && uniqueIds.Contains(l.CandidateId))
                .ToListAsync();

            if (existingLeaves.Any())
            {
                _appDbContext.Leaves.RemoveRange(existingLeaves);
            }

          
            var newLeaves = uniqueIds.Select(id => new Leave
            {
                CandidateId = id,
                LeaveTypeId = (int)dto.LeaveTypeId, 
                LeaveDate = leaveDate,
                CreatedAt = DateTime.UtcNow
            });

            await _appDbContext.Leaves.AddRangeAsync(newLeaves);
            await _appDbContext.SaveChangesAsync();

            return new APIResponse<string>
            {
                Sucess = true,
                Message = existingLeaves.Any() ? "Leave updated" : "Leave assigned",
                StatusCode = 200
            };
        }

        public async Task<APIResponse<string>> RemoveLeave(int candidateId)
        {
            var today = DateTime.UtcNow.Date;
            var leaveDate = DateOnly.FromDateTime(today);
            var existingLeave = await _appDbContext.Leaves
                .FirstOrDefaultAsync(l => l.CandidateId == candidateId && l.LeaveDate == leaveDate);
            if (existingLeave == null)
            {
                return new APIResponse<string>
                {
                    Sucess = false,
                    Message = "No leave found for the candidate today",
                    StatusCode = 404,
                    Data = null
                };
            }
            _appDbContext.Leaves.Remove(existingLeave);
            await _appDbContext.SaveChangesAsync();
            return new APIResponse<string>
            {
                Sucess = true,
                Message = "Leave removed successfully",
                StatusCode = 200,
                Data = null
            };
        }
        public async Task<List<LeaveType>> GetAllLeaveTypes()
        {
            return await _appDbContext.LeaveTypes

                .ToListAsync();

        }
    }



}

