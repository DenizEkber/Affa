using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFFA.src.DEPO.DataBase.Entity;

namespace AFFA.src.DEPO.DataBase.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.UserId);

            builder.Property(u => u.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
                .IsRequired();

            builder.Property(u => u.PasswordSalt)
                .IsRequired();

            builder.Property(u => u.Role)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
