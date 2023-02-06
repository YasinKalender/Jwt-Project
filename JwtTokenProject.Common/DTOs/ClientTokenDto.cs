namespace JwtTokenProject.Common.DTOs
{
    public class ClientTokenDto
    {
        public string AccessToken { get; set; }
        public DateTime ExprationTime { get; set; }
    }
}
