namespace BioMatricAttendance.DTOsModel
{
    public class GetDeviceNameDto
    {
        public int Id { get; set; }
        public long DeviceId { get; set; }
        public string SessionId { get; set; }
        public bool isRegistered { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
