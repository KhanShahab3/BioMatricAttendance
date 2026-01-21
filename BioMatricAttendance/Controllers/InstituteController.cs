using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstituteController : ControllerBase
    {
        private readonly IInstituteService _instituteService;
        public InstituteController(IInstituteService instituteService)
        {
            _instituteService = instituteService;

        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllInstitutes()
        {
            var institutes = await _instituteService.GetInstitutes();
            if (institutes.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "No institutes found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Institutes fetched successfully",
                Data = institutes,
                StatusCode = 200
            });


        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInstituteById(int id)
        {
            var institute = await _instituteService.GetInstitute(id);
            if (institute == null)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Institute not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Institute fetched successfully",
                Data = institute,
                StatusCode = 200
            });
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateInstitute(CreateInstituteDto institute)
        {
            var createdInstitute = await _instituteService.CreateInstitute(institute);
            if (createdInstitute == null)
            {
                BadRequest(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Institute are not created",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Institute are created succesfully",
                StatusCode = 201,
               Data = createdInstitute  
            });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateInstitute(Institute institute)
        {
            var updatedInstitute = await _instituteService.UpdateInstitute(institute);
            if (updatedInstitute == null)
            {
                BadRequest(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Institute are not updated",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Institute are updated succesfully",
                StatusCode = 200
            });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteInstitute(int id)
        {
            var isDeleted = await _instituteService.RemoveInstitute(id);
            if (!isDeleted)
            {
                BadRequest(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Institute are not deleted",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Institute are deleted succesfully",
                StatusCode = 200
            });
        }
    }
}
