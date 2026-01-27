namespace BioMatricAttendance.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long DeviceId { get; set; }
        public int DeviceUserId { get; set; }
        public int CourseId { get; set; }
        public string Previliges { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Enable { get; set; }
        public DateTime PeriodStart {  get; set; }
        public DateTime PeriodEnd { get; set; }
        public string PeriodUse {  get; set; }
    }
}
