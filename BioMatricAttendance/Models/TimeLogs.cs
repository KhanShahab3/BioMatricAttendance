

namespace BioMatricAttendance.Models
{
    public class TimeLogs
    {
    public int Id { get; set; }
    public int DeviceUserId { get; set; }
    public long DeviceId { get; set; }
    public DateTime PunchTime { get; set; }
    public string AttendType { get; set; }
    public bool IsProcessed { get; set; } = false;
    public DateTime CreatedAt { get; set; }= DateTime.Now;

    }
}
