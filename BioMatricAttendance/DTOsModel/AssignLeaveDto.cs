namespace BioMatricAttendance.DTOsModel
{
    public class AssignLeaveDto
    {
        //int assignedBy

         public int LeaveTypeId { get; set; }
        //public DateTime StartDate {  get; set; }
        //public DateTime EndDate { get; set; }
        public DateTime LeaveDate { get; set; }
        public List<int> CandidateIds {  get; set; }
    }
}
