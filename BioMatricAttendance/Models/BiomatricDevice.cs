namespace BioMatricAttendance.Models
{
    public class BiomatricDevice
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public long DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int InstituteId { get; set; }
        public Institute Institute { get; set; }
        public bool IsDeleted { get; set; }= false;
    }
}
