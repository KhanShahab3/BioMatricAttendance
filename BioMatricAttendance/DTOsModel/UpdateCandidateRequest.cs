using BioMatricAttendance.Models;

namespace BioMatricAttendance.DTOsModel
{
    public class UpdateCandidateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string? EmployeeId {  get; set; }
        public Gender Gender { get; set; }

       
    }
}
