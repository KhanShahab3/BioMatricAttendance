namespace BioMatricAttendance.DTOsModel
{
    public class DistrictDashboardDto
    {
        public DistrictDashboardSummaryDto Summary { get; set; }
        public List<InstituteComparisonDto> Institutes { get; set; }
    }
}
