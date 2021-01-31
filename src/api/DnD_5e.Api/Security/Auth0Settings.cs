namespace DnD_5e.Api.Security
{
    public class Auth0Settings
    {
        public static string ConfigSection { get; } = "Auth0Settings";

        public string Domain { get; set; }
        public string Audience { get; set; }
        public string ClientId { get; set; }
        public string Authority { get; set; }
    }
}
