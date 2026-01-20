namespace BioMatricAttendance.Models
{
    public class Region
    {
        public int Id { get; set; }
        public string RegionName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public bool IsDeleted { get; set; } = false;

        public ICollection<Institute> Institutes { get; set; }
    }
}
