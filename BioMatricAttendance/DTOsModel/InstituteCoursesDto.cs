namespace BioMatricAttendance.DTOsModel
{
    public class InstituteCoursesDto
    {
        public int Id { get; set; } 
        public string InstituteName { get; set; }
        public GetRegionNameDto RegionName { get; set; }
        public int TotalCourses { get; set; }
    }
}
