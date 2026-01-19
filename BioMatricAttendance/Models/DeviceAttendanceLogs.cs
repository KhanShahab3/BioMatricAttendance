using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BioMatricAttendance.Models
{
    public class DeviceAttendanceLogs
    {
        public int Id { get; set; }
        public long DeviceUserId { get; set; }
        public string DeviceName { get; set; }
        public long DeviceId { get; set; }
        public DateTime PunchTime { get; set; }
        public string AttendType { get; set; }

        public bool IsProcessed { get; set; } = false;
        public DateTime CreatedAt { get; set; }

    }
}
