using System.Text.Json.Serialization;

namespace BioMatricAttendance.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }   
        public bool IsDeleted { get; set; } = false;
        public int RoleId { get; set; }
        public int? InstituteId { get; set; }
        public int? RegionId { get; set; }
     

        public int? DistrictId { get; set; }
        //public int? HQ { get; set; }
        [JsonIgnore]
        public Role ?Role { get; set; }
        [JsonIgnore]
        public ICollection<Candidate>? Candidates { get; set; }
    }
}

