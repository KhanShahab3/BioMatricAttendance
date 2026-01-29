using System.Diagnostics.Contracts;

namespace BioMatricAttendance.DTOsModel
{
    public class CourseCandidateDto
    {
        public int CourseId {  get; set; }
        public List<int> StudentIds { get; set; }
    }
}
