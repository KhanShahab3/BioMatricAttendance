namespace BioMatricAttendance.Models
{
    public class BiomatricDevice
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public string ModelNumber { get; set; }
        public string Location { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public int InstituteId { get; set; }
    }
}
