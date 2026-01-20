namespace BioMatricAttendance.DTOsModel
{
    public class InstituteAttendanceRowDto
    {
        public string InstituteName { get; set; }
        public string Region { get; set; }
        public int Total { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public decimal AttendancePercentage { get; set; }
        public string Status { get; set; }
    }
}
