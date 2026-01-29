using BioMatricAttendance.Models;

namespace BioMatricAttendance.Response
{
    public class InstituteFacultyResponse
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public Gender gender { get; set; }
    }
}
