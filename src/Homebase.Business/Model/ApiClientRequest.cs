namespace Homebase.Business.Model
{
    public class ApiClientRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string BaseAddress { get; set; }
        public string RequestUri { get; set; }
        public string Content { get; set; }
    }
}
