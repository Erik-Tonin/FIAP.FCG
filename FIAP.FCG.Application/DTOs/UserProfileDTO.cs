namespace FIAP.FCG.Application.DTOs
{
    public class UserProfileDTO
    {
        public Guid? Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public DateTime Birthday { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
