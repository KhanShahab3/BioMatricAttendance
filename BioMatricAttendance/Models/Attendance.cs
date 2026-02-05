namespace BioMatricAttendance.Models
{
    public class Attendance
    {
        public long Id { get; set; }
      
        public DateTime Date { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int InstituteId { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }
        public Institute Institute { get; set; }
    }
}
