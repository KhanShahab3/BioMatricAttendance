namespace BioMatricAttendance.DTOsModel
{
    public class AttendanceRecordDTO
    {
        public int PersonId { get; set; }               
        public string PersonName { get; set; }         
        public string Role { get; set; }                
        public int TotalPresent { get; set; }           
        public int TotalAbsent { get; set; }            
        public double PercentagePresent { get; set; }   
        public DateTime FirstCheckIn { get; set; }      
        public DateTime LastCheckOut { get; set; }      
        public int DeviceId { get; set; }              
        public int InstituteId { get; set; }
    }
}
