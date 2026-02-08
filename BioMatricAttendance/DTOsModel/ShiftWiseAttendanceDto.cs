namespace BioMatricAttendance.DTOsModel
{
    public class ShiftWiseAttendanceDto
    {

        public int ShiftId { get; set; }
        public string ShiftName { get; set; }

        public int StudentPresent { get; set; }
        public int StudentAbsent { get; set; }

        public int FacultyPresent { get; set; }
        public int FacultyAbsent { get; set; }
    }
}
