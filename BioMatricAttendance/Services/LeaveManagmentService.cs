using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Helper;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Services
{ 
    public class LeaveManagmentService:ILeaveManagmentService
    {
        private readonly AppDbContext _appDbContext;
        public LeaveManagmentService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<List<AbsentCandidateDto>> GetAbsentCandidates(
         int? regionId,
         int? instituteId)
        {
            var deviceIds = new List<long>();
            var today = DateTime.UtcNow.Date; 
            var (startUtc, endUtc) = DateTimeHelper.GetUtcRangeForPakistanDate(today, today);

            
            if (regionId > 0 && instituteId > 0)
            {
                deviceIds = await _appDbContext.Institutes
                    .Where(i => i.Id == instituteId && i.RegionId == regionId && !i.IsDeleted)
                    .SelectMany(i => i.BiomatricDevices.Where(d => d.isRegistered).Select(d => d.DeviceId))
                    .ToListAsync();
            }
            else
            {
                deviceIds = await _appDbContext.Institutes
                    .Where(i => !i.IsDeleted)
                    .SelectMany(i => i.BiomatricDevices.Where(d => d.isRegistered).Select(d => d.DeviceId))
                    .ToListAsync();
            }

            if (!deviceIds.Any())
                return new List<AbsentCandidateDto>();

           
            var allCandidates = await _appDbContext.Candidates
                .Where(c => c.Enable && deviceIds.Contains(c.DeviceId))
                .ToListAsync();

            var presentDeviceUserIds = await _appDbContext.TimeLogs
                .Where(tl => deviceIds.Contains(tl.DeviceId)
                             && tl.PunchTime >= startUtc
                             && tl.PunchTime < endUtc)
                .Select(tl => tl.DeviceUserId)
                .Distinct()
                .ToListAsync();

        
            var strtdate = DateOnly.FromDateTime(startUtc.Date);
            var enddate = DateOnly.FromDateTime(endUtc.Date);

            var leaves = await _appDbContext.Leaves
                .Where(l => l.LeaveDate >= strtdate && l.LeaveDate <= enddate)
                .Select(l => new { l.CandidateId, l.LeaveTypeId })
                .ToListAsync();

            var leaveTypes = await _appDbContext.LeaveTypes
                .Select(lt => new { lt.Id, lt.TypeName })
                .ToListAsync();

           
            var absentCandidates = allCandidates
                .Where(c => !presentDeviceUserIds.Contains(c.DeviceUserId))
                .Select(c =>
                {
                    var leave = leaves.FirstOrDefault(l => l.CandidateId == c.Id);
                    var leaveTypeName = leave != null
                        ? leaveTypes.FirstOrDefault(lt => lt.Id == leave.LeaveTypeId)?.TypeName
                        : null;

                    return new AbsentCandidateDto
                    {
                        Id = c.Id,
                        Name = c.Name,
                        DeviceId = c.DeviceId,
                        DeviceUserId = c.DeviceUserId,
                        IsOnLeave = leave != null,
                        LeaveTypeName = leaveTypeName,
                        LeaveTypeId= leave?.LeaveTypeId,
                        gender = c.gender
                    };
                })
                .ToList();

            return absentCandidates;
        }






        public async Task<APIResponse<string>> AssignLeave(AssignLeaveDto dto)
        {
            var (startDate, _) = DateTimeHelper.GetUtcRangeForPakistanDate(dto.LeaveDate, null);
            var leaveDate = DateOnly.FromDateTime(startDate);

            // Candidate Validation
            var validCandidateIds = await _appDbContext.Candidates
                .Where(c => dto.CandidateIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToListAsync();

            if (validCandidateIds.Count != dto.CandidateIds.Count)
            {
                return new APIResponse<string>
                {
                    Sucess = false,
                    Message = "Invalid candidate IDs",
                    StatusCode = 400,
                    Data = null
                };
            }

          
            var existingLeaves = await _appDbContext.Leaves
                .Where(l => l.LeaveDate == leaveDate &&
                            dto.CandidateIds.Contains(l.CandidateId))
                .ToListAsync();

            _appDbContext.Leaves.RemoveRange(existingLeaves);
            int deleteCount = existingLeaves.Count;

            if (dto.LeaveTypeId > 0)
            {
                var leaveTypeExists = await _appDbContext.LeaveTypes
                    .AnyAsync(lt => lt.Id == dto.LeaveTypeId);

                if (!leaveTypeExists)
                {
                    return new APIResponse<string>
                    {
                        Sucess = false,
                        Message = "Invalid leave type",
                        StatusCode = 400,
                        Data = null
                    };
                }

                var newLeaves = dto.CandidateIds.Select(id => new Leave
                {
                    CandidateId = id,
                    LeaveTypeId = dto.LeaveTypeId,
                    LeaveDate = leaveDate,
                    CreatedAt = DateTime.UtcNow
                });

                await _appDbContext.Leaves.AddRangeAsync(newLeaves);
            }

            await _appDbContext.SaveChangesAsync();

           
            if (deleteCount > 0 && dto.LeaveTypeId ==0)
            {
                return new APIResponse<string>
                {
                    Sucess = true,
                    Message = "Record deleted",
                    StatusCode = 200,
                    Data = null
                };
            }

            if (deleteCount > 0 && dto.LeaveTypeId > 0)
            {
                return new APIResponse<string>
                {
                    Sucess = true,
                    Message = "Leave update",
                    StatusCode = 200,
                    Data = null
                };
            }

            return new APIResponse<string>
            {
                Sucess = true,
                Message = "Leave Remove",
                StatusCode = 200,
                Data = null
            };
        }

    }



}

