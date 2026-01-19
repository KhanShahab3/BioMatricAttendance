namespace BioMatricAttendance.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public long UserDeviceId { get; set; }
        public int instituteId { get; set; }
        public string CandidateType { get; set; } //prevlige
    }
}
