namespace Ecommerce.Services.Responses
{
    public class AuthResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public AuthData Data { get; set; }
    }

    public class AuthData
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public bool IsAdmin { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
