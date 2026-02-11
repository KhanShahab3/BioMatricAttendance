using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShiftController : ControllerBase
    {
        private readonly IShiftService _shiftService;

        public ShiftController(IShiftService shiftService) {
        
        _shiftService = shiftService;
        }
        [HttpPost("assign")]
        public async Task<IActionResult> AssignShift( AssignShiftDto dto)
        {
            if (dto == null || !dto.CandidateIds.Any())
                return BadRequest("No candidates selected");

            await _shiftService.AssignShiftAsync(dto);
            return Ok(new { Message = "Shifts assigned successfully" });
        }
        [HttpPost("createShiftType")]
        public async Task<IActionResult> CreateShiftType(ShiftType shift)
        {
            if (shift == null)
                return BadRequest("Invalid shift type data");
            await _shiftService.CreateShift(shift);
            return Ok(new { Message = "Shift type created successfully" });
        }
        [HttpGet("getAllShifts")]
        public async Task<IActionResult> GetAllShifts()
        {
            var shifts = await _shiftService.GetAllShifts();
            return Ok(shifts);
        }
        [HttpGet("getShiftById/{id}")]
        public async Task<IActionResult> GetShiftById(int id)
        {
            var shift = await _shiftService.GetShiftById(id);
            if (shift == null)
                return NotFound("Shift type not found");
            return Ok(shift);
        }

        [HttpPut("updateShift")]
  public async Task<IActionResult>UpdateShift(ShiftType shift)
        {
            if (shift == null || shift.Id <= 0)
                return BadRequest("Invalid shift type data");
            var updatedShift = await _shiftService.UpdateShiftType(shift);  
            return Ok(new { Message = "Shift type updated successfully", Data = updatedShift });
        }
        [HttpDelete("deleteShift/{id}")]
        public async Task<IActionResult> DeleteShift(int id)
        {
            if (id <= 0)
                return BadRequest("Invalid shift type ID");
            await _shiftService.DeleteShiftType(id);
            return Ok(new { Message = "Shift type deleted successfully" });
        }


        [HttpGet("CandidateShift")]

        public async Task<IActionResult> GetShiftCandidate([FromQuery] int? instituteId,
        [FromQuery] int? regionId
       )
        {
            var candidates = await _shiftService.GetCandidatesWithShift(instituteId, regionId);
            if(candidates == null)
            {
                return NotFound("No candiates found");
            }
            return Ok(candidates);
        }

    }
}
