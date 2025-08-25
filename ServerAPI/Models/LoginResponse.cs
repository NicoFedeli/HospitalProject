namespace HospitalAPI.Models
{
    public interface ILoginResponse
    { 
        int Id { get; set; }
        string Username { get; set; }
        string Role { get; set; }
        string? Admin { get; set; } // SOlO PER DOCTOR e NURSE
        string? Token { get; set; }
    }

    public class LoginResponseData : ILoginResponse
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public string? Admin { get; set; }
        public string? Token { get; set; }
    }

    public class LoginResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public ILoginResponse Data { get; set; }
    }
}
