namespace JwtTokenProject.Common.DTOs.Model
{
    public class Client
    {
        public string ClientId { get; set; }
        public string Secret { get; set; }
        public List<String> Audiences { get; set; }
    }
}
