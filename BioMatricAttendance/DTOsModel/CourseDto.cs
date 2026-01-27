using BioMatricAttendance.Models;
using System.ComponentModel.DataAnnotations;

namespace BioMatricAttendance.DTOsModel
{
    public class CourseDto
    {

        public int Id { get; set; }

        [Required(ErrorMessage ="CourseName is Required")]
        [StringLength(100, MinimumLength = 3)]
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public string Duration { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a valid Institute")]
        public int InstituteId { get; set; }

        
    }
}
