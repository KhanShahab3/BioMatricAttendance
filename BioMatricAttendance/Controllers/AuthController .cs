using BioMatricAttendance.AttendenceContext;
using BioMatricAttendance.DTOsModel;
using BioMatricAttendance.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;


        public AuthController(AppDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.AppUsers
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email && !u.IsDeleted);

            if (user == null)
                return Unauthorized("User not found");

            if (user.Password != dto.Password)
                return Unauthorized("Invalid credentials");

            var token = _jwtService.GenerateToken(user);

            return Ok(new
            {
                Token = token,
                UserId = user.Id,
                Name = user.Name,
                Role = user.Role?.RoleName,
                RegionId=user.RegionId
                 
            });
        }
    }

}

