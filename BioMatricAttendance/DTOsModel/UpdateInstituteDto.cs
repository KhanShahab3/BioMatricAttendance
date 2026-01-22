namespace BioMatricAttendance.DTOsModel
{
    public class UpdateInstituteDto
    {
        public int Id { get; set; } 
        public string InstituteName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public int RegionId { get; set; }
        public List<int> DeviceIds { get; set; }
        public DateTime UpdatedAt { get; set; }=DateTime.Now;
    }
}
