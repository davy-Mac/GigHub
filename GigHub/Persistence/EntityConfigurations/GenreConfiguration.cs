using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class GenreConfiguration : EntityTypeConfiguration<Genre>// inherits from EntityTypeConfiguration
    {
        public GenreConfiguration()
        {
            Property(g => g.Name) //property configuration
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}