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
                    sucess = false,
                    Message = "user are not created",
                    Data = new { },
                    StatusCode = 400
                });
            }
            return Ok(new APIResponse<object>
            {
                sucess = true,
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
                    sucess = false,
                    Message = "User not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                sucess = true,
                Message = "User fetched sucessfully",
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
                    sucess = false,
                    Message = "No users found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                sucess = true,
                Message = "Users fetched sucessfully",
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
                    sucess = false,
                    Message = "User not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                sucess = true,
                Message = "User updated sucessfully",
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
                    sucess = false,
                    Message = "User not found",
                    Data = new { },
                    StatusCode = 404
                });
            }
            return Ok(new APIResponse<object>
            {
                sucess = true,
                Message = "User deleted sucessfully",
                StatusCode = 200
            });
        }
    }
}
