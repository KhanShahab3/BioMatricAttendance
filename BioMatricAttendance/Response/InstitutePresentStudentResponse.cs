namespace BioMatricAttendance.Response
{
    public class InstitutePresentStudentResponse
    {
        
        public int DeviceUserId {  get; set; }
        public long DeviceId {  get; set; }
        public DateTime? PunchDate {  get; set; }
        public string StudentName {  get; set; }
        public string FirstPunch { get; set; }
        public string LastPunch { get; set; }
        public int CandidateId { get; set; }
        public bool IsPresent { get; set; }  

    }
    public class InstitutePresentFaculityResponse
    {
        
        public int DeviceUserId { get; set; }
        public long DeviceId { get; set; }
        public DateTime? PunchDate { get; set; }
        public string FaculityName { get; set; }
        public string FirstPunch { get; set; }
        public string LastPunch { get; set; }
        public bool IsPresent { get; set; }
        public int CandidateId { get; set; }

    }
}
