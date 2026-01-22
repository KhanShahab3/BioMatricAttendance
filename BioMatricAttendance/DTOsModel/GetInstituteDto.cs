using BioMatricAttendance.Models;

namespace BioMatricAttendance.DTOsModel
{
    public class GetInstituteDto
    {

      public int Id { get; set; }
        public string InstituteName { get; set; }
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
     
        public GetRegionNameDto Region { get; set; }

        public int DeviceCount { get; set; }    
    }
}
