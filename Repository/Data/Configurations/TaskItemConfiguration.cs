using Core.Entities;
using Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Data.Configurations
{
    public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {

            builder.Property(t => t.Status)
                   .HasConversion(new EnumToStringConverter<Status>());


            builder.Property(t => t.Priority)
                   .HasConversion(new EnumToStringConverter<Priority>());

            builder.Property(t => t.UserId).IsRequired();

        }
    }
}
