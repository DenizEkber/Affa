using AFFA.src.DEPO.DataBase.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AFFA.src.DEPO.DataBase.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.CustomerName)
                   .IsRequired()
                   .HasMaxLength(100);
        }
    }
}
