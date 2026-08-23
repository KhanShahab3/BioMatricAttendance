namespace BioMatricAttendance.DTOsModel
{
    public class CandidateDTO
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string? Gender { get; set; } 
        public long DeviceId { get; set; }
        public string? Previliges { get; set; }
    }
}
