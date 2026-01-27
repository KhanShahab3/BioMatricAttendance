using BioMatricAttendance.Models;
using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.AttendenceContext
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
        }

        public DbSet<User> AppUsers { get; set; }
        public DbSet<Institute> Institutes { get; set; }
        public DbSet<BiomatricDevice> BiomatricDevices { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Region>Regions { get; set; }
        public DbSet<TimeLogs> TimeLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BiomatricDevice>()
                .HasIndex(d => d.DeviceId)
                .IsUnique();
            modelBuilder.Entity<Candidate>()
      .HasIndex(c => new { c.DeviceId, c.DeviceUserId })
      .IsUnique()
      .HasDatabaseName("UX_Candidate_Device_DeviceUser");

            modelBuilder.Entity<TimeLogs>()
                .HasIndex(t=>new {t.DeviceId,t.DeviceUserId,t.CreatedAt}).IsUnique();
            modelBuilder.Entity<TimeLogs>()
    .HasIndex(t => new { t.DeviceId, t.DeviceUserId, t.PunchTime })
    .IsUnique()
    .HasDatabaseName("UX_TimeLogs_Device_User_CreatedAt");
            base.OnModelCreating(modelBuilder);





        }

    }
}
