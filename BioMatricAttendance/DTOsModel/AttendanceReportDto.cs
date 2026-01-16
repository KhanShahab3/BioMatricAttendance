namespace BioMatricAttendance.DTOsModel
{
    public class AttendanceReportDto
    {
        public DateTime From { get; set; }                     
        public DateTime To { get; set; }                      
        public int TotalFaculty { get; set; }                   
        public int TotalStudents { get; set; }                 
        public List<AttendanceRecordDTO> FacultyRecords { get; set; }  
        public List<AttendanceRecordDTO> StudentRecords { get; set; }
    }
}
