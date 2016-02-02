namespace Homebase.Business.Model
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public bool IsEmpty
        {
            get { return string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password); }
        }
    }
}
