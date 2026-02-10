namespace BioMatricAttendance.DTOsModel
{
    public class CandidateWithShiftDto
    {
        public int CandidateId { get; set; }
        public string Name { get; set; }
        public int? ShiftId { get; set; }      
        public string? ShiftName { get; set; }
        public bool IsAssigned { get; set; }
    }
}
