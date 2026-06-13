using Core.Entities.Identity;
using Core.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Repository.Data.Identity
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser>
    {
        public AppIdentityDbContext(DbContextOptions<AppIdentityDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // تحويل UserType enum إلى string أو int في قاعدة البيانات
            builder.Entity<AppUser>()
                   .Property(u => u.UserType)
                   .HasConversion(new EnumToStringConverter<UserType>());
        }
    }
}