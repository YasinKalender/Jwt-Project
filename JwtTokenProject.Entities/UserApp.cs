using Microsoft.AspNetCore.Identity;

namespace JwtTokenProject.Entities
{
    public class UserApp : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<Product> Products { get; set; }
    }
}
