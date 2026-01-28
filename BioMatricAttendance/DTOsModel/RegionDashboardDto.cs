namespace BioMatricAttendance.DTOsModel
{
    public class RegionDashboardDto
    {
        //public int RegionId { get; set; }
        //public int? DistrictId { get; set; }
        public DateTime Date { get; set; }

        public RegionDashboardSummaryDto Summary { get; set; }
        public List<InstituteComparisonDto> Institutes { get; set; } = new();
    }
}
