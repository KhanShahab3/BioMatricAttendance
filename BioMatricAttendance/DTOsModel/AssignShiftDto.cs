using BioMatricAttendance.Models;

namespace BioMatricAttendance.DTOsModel
{
    public class AssignShiftDto
    {

        public List<int> CandidateIds { get; set; }
        public int  ShiftId { get; set; }
        public DateTime ShiftDate { get; set; }
        //public int AssignedBy { get; set; }
    }
}
