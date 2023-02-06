namespace JwtTokenProject.Common.DTOs
{
    public class TokenDto
    {
        public string AccessToken { get; set; }
        public DateTime ExprationTime { get; set; }
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExprationTime { get; set; }
    }
}
