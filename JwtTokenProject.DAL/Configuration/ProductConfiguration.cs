using JwtTokenProject.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace JwtTokenProject.DAL.Configuration
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Name).HasColumnName("Name").IsRequired().HasMaxLength(20);
            builder.Property(i => i.Price).HasColumnName("Price").HasColumnType("decimal(18, 2)");

            //builder.HasOne(i => i.UserApp).WithMany(i => i.Products).HasForeignKey(i => i.UserId);
        }
    }
}
