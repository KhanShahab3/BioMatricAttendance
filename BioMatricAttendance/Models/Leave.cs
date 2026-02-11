namespace BioMatricAttendance.Models
{
    public class Leave
    {
        public int Id { get; set; } 
        public int CandidateId { get; set; }
        public int LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateOnly LeaveDate { get; set; }          
        

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
