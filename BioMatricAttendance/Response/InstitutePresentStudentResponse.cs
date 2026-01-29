namespace BioMatricAttendance.Response
{
    public class InstitutePresentStudentResponse
    {
        public int id {  get; set; }
        public int DeviceUserId {  get; set; }
        public long DeviceId {  get; set; }
        public DateTime PunchDate {  get; set; }
        public string StudentName {  get; set; }
        public string FirstPunch { get; set; }
        public string LastPunch { get; set; }

    }
    public class InstitutePresentFaculityResponse
    {
        public int id { get; set; }
        public int DeviceUserId { get; set; }
        public long DeviceId { get; set; }
        public DateTime PunchDate { get; set; }
        public string FaculityName { get; set; }
        public string FirstPunch { get; set; }
        public string LastPunch { get; set; }

    }
}
