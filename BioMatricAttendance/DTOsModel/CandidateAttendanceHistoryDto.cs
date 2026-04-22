namespace BioMatricAttendance.DTOsModel
{
    public class CandidateAttendanceHistoryDto
    {
        public int CandidateId { get; set; }

     
        public string Name { get; set; } = string.Empty;

        
        public int DeviceUserId { get; set; }

      
        public long DeviceId { get; set; }

        public DateTime StartDate { get; set; }

       
        public DateTime EndDate { get; set; }

        public string Previliges { get; set; }

        
        public List<DailyAttendanceDto> Days { get; set; } = new();

        
        public int TotalDays => Days?.Count ?? 0;
        public int DaysPresent => Days?.Count(d => d.Present) ?? 0;
        public int DaysAbsent => TotalDays - DaysPresent;

        
   
    }
}
