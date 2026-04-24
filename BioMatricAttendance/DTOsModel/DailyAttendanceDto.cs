namespace BioMatricAttendance.DTOsModel
{
    public class DailyAttendanceDto
    {
        public DateTime Date { get; set; }

       
        public bool Present { get; set; }

     
        public string? FirstPunch { get; set; }

       
        public string? LastPunch { get; set; }
    }
}
