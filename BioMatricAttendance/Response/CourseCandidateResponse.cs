namespace BioMatricAttendance.Response
{
    public class CourseCandidateResponse
    {
        public string CourseName { get; set; }
        public List<CourseCandidatesinfoResponse> Candaitesinfos { get; set; }
    }
    public class CourseCandidatesinfoResponse
    {
        public int CourseCandidateId { get; set; }
        public int CandidateId {  get; set; }
        public string CandidateName { get; set; }
    }
}
