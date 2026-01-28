namespace BioMatricAttendance.Models
{
    public class District
    {
        public int Id { get; set; }
        public string DistrictName { get; set; }
         public int RegionId { get; set; }

        public List<Institute> Institutes { get; set; }
    }
}
