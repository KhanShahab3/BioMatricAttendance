using System.ComponentModel.DataAnnotations;

namespace BioMatricAttendance.Models
{
    public class ShiftType
    {
        [Key]
        public int Id { get; set; }
        public string ShiftName { get; set; }   
        public TimeSpan StartTime {  get; set; }
        public TimeSpan EndTime { get; set; }
        public bool isDeleted { get; set; } = false;    
    }
}
