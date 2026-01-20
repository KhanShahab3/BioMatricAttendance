using Microsoft.EntityFrameworkCore;

namespace BioMatricAttendance.Models
{
    public class BiomatricDevice
    {
        public int Id { get; set; }
        public long DeviceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
      public string SessionId {  get; set; }
        public bool IsDeleted { get; set; } = false;
        public int InstituteId { get; set; }
        public Institute Institute { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
    }
}
