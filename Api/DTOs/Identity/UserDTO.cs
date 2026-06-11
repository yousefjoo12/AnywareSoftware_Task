namespace API.DTOs.Identity
{
    public class UserDTO
    {
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
