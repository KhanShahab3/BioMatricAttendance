namespace BioMatricAttendance.DTOsModel
{
    public class AttendanceReportDto
    {
        public int CandidateId { get; set; }
        public string CandidateName { get; set; }
        public string CandidateType { get; set; } 
        public DateTime Date { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public TimeSpan TotalHours { get; set; }
    }
}
