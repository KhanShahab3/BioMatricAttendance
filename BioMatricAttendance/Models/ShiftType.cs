namespace BioMatricAttendance.Models
{
    public class ShiftType
    {
        public int Id { get; set; }
        public string ShiftName { get; set; }   
        public TimeSpan StartTime {  get; set; }
        public TimeSpan EndTime { get; set; }
    }
}
