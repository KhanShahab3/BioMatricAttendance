using BioMatricAttendance.Models;

namespace BioMatricAttendance.DTOsModel
{
    public class AbsentCandidateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long DeviceId { get; set; }
        public long DeviceUserId { get; set; }
        public int? LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }
        public bool IsOnLeave { get; set; }
        public Gender? gender { get; set; }
    }
}
