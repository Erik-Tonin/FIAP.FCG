namespace FIAP.FCG.Domain.Entities
{
    public class KeycloakOptions
    {
        public string? ServerUrl { get; set; }
        public string? Realm { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }
        public string? TokenUrl { get; set; }
    }
}
