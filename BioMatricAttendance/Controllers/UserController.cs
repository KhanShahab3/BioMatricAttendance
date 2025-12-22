using BioMatricAttendance.Models;
using BioMatricAttendance.Response;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(User user)
        {
            var createdUser = await _userService.CreateUser(user);
            if (createdUser == null) {
                BadRequest(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "user are not created",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "User are created succesfully",
                StatusCode = 201
            });
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "User not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "User fetched successfully",
                Data = user,
                StatusCode = 200
            });
        }
        [HttpGet("all")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetUsers();
            if(users.Count == 0)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "No users found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "Users fetched successfully",
                Data = users,
                StatusCode = 200
            });
        }
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            var updatedUser = await _userService.UpdateUser(user);
            if (updatedUser == null)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "User not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "User updated successfully",
                Data = updatedUser,
                StatusCode = 200
            });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _userService.DeleteUser(id);
            if (!result)
            {
                return NotFound(new APIResponse<object>
                {
                    Sucess = false,
                    Message = "User not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                Sucess = true,
                Message = "User deleted successfully",
                StatusCode = 200
            });
        }
    }
}
