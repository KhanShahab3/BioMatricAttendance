using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {
        private readonly IBioMatricDeviceService _deviceService;
        public DeviceController(IBioMatricDeviceService deviceService)
        {
            _deviceService = deviceService;
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllDevices()
        {
            var devices = await _deviceService.GetDevices();
            if(devices.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "No devices found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Devices fetched successfully",
                Data = devices,
                StatusCode = 200
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDeviceById(int deviceId)
        {
            var device = await _deviceService.GetDevice(deviceId);
            if (device == null)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Device not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Device fetched successfully",
                Data = device,
                StatusCode = 200
            });
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateDevice(BiomatricDevice device)
        {
            var createdDevice = await _deviceService.CreateDevice(device);
            if (createdDevice == null)
            {
                BadRequest(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Device are not created",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Device are created succesfully",
                StatusCode = 201
            });
        }


        [HttpPut("update")]
        public async Task<IActionResult> UpdateDevice(BiomatricDevice device)
        {
            var updatedDevice = await _deviceService.UpdateDevice(device);
            if (updatedDevice == null)
            {
                return BadRequest(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Device update failed",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Device updated successfully",
                Data = updatedDevice,
                StatusCode = 200
            });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteDevice(int id)
        {
            var isDeleted = await _deviceService.RemoveDevice(id);
            if (!isDeleted)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "Device not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Device deleted successfully",
                StatusCode = 200
            });
        }


    }
}
