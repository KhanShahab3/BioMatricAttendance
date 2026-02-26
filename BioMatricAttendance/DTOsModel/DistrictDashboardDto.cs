namespace BioMatricAttendance.DTOsModel
{
    public class DistrictDashboardDto
    {
        public DistrictDashboardSummaryDto Summary { get; set; }
        public List<InstituteComparisonDto> Institutes { get; set; }

        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
