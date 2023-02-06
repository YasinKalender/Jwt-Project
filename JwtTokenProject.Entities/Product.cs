using System.ComponentModel.DataAnnotations.Schema;

namespace JwtTokenProject.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string UserId { get; set; }
        public virtual UserApp UserApp { get; set; }
    }
}
