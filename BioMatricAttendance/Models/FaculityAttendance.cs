namespace BioMatricAttendance.Models
{
    public class FaculityAttendance
    {

        //institute Name,region Name,total faculity,AvgPresentFaculity,totalpresentDays,AvgAttendancePercentage
        public int Id { get; set; }
        //forign key of staff
        public int DeviceId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime ?CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Status { get; set; }
        public double WorkHours { get; set; }
        public double OverTimeHours { get; set; }
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
        public int FaculityId { get; set; }
        public Faculity Faculity { get; set; }
        public int InstituteId { get; set; }
        public Institute Institute { get; set; }

    }
}