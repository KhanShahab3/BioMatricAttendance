namespace BioMatricAttendance.DTOsModel
{
    public class CreateInstituteDto
    {

        public string InstituteName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public int RegionId { get; set; }
        public List<int> DeviceIds { get; set; }
        public DateTime CreatedAt { get; set; }=DateTime.Now;

    }
}
