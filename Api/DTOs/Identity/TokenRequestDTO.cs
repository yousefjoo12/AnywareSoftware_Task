namespace API.DTOs.Identity
{
    public class TokenRequestDTO
    {
        public string Email { get; set; }
        public string RefreshToken { get; set; }
    }
}
