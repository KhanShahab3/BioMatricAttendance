namespace BioMatricAttendance.DTOsModel
{
    public class FacultyAttendanceSummaryDto
    {

        public int TotalFaculty { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int OnLeave { get; set; }
        public int Overtime { get; set; }
    }
}
