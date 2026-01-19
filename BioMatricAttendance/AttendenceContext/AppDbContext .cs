using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.AttendenceContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<BiomatricDevice> BiomatricDevices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Region>Regions { get; set; }
        public DbSet<DeviceAttendanceLogs> DeviceAttendanceLogs { get; set; }

    }
}
