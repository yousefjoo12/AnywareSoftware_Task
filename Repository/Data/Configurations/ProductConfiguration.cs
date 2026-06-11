using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Configurations
{
    //public class ProductConfiguration : IEntityTypeConfiguration<Product>
    //{
    //    public void Configure(EntityTypeBuilder<Product> builder)
    //    {
    //        builder.Property(x => x.ID)
    //               .HasColumnName("ProductId");

    //        builder.Property(p => p.Name)
    //               .IsRequired()
    //               .HasMaxLength(100);

    //        builder.Property(p => p.Description)
    //              .IsRequired();

    //        builder.Property(x => x.Price)
    //               .HasColumnType("decimal(18,2)"); 
    //    }
   // }
}
