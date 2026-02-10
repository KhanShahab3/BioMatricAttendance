namespace BioMatricAttendance.Models
{
    public class CandidateShift
    {
        public int Id { get; set; }
        public int CandidateId { get; set; }
        public Candidate Candidate { get; set; }

       public int ShiftId { get; set; }
        public ShiftType Shift { get; set; }

        //public int AssignedBy { get; set; }    
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }



}
