namespace BioMatricAttendance.Response
{
    public class APIResponse<T>
    {
        public bool Sucess { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public T Data { get; set; }
    }

}
