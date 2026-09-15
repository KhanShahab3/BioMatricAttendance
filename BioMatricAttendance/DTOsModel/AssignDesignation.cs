using BioMatricAttendance.Models;

namespace BioMatricAttendance.DTOsModel
{
    public class AssignDesignation
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public Gender Gender { get; set; }
    }
}
