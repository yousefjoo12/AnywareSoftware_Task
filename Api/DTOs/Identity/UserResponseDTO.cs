namespace API.DTOs.Identity
{
    public class UserResponseDTO
    {
        public string id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string UserType { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
