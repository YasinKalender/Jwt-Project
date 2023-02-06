namespace JwtTokenProject.Entities
{
    public class UserRefreshToken
    {
        public string UserId { get; set; }
        public string RefreshToken { get; set; }
        public DateTime Expration { get; set; }
    }
}
