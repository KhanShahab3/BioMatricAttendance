namespace BioMatricAttendance.DTOsModel
{
    public class DailyAttendanceDto
    {
        public DateTime Date { get; set; }

        // true if there is at least one punch on this date
        public bool Present { get; set; }

        // PK-local time formatted "HH:mm:ss", null when absent
        public string? FirstPunch { get; set; }

        // PK-local time formatted "HH:mm:ss", null when absent
        public string? LastPunch { get; set; }
    }
}
