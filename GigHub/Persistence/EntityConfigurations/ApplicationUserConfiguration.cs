using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class ApplicationUserConfiguration : EntityTypeConfiguration<ApplicationUser>// inherits from EntityTypeConfiguration
    {                                                                                   // which is a generic class that takes ApplicationUser as generic parameter
        public ApplicationUserConfiguration()
        {
            Property(u => u.Name) //property configuration
                .IsRequired()
                .HasMaxLength(100);

            HasMany(u => u.Followers) //relationship configuration
                .WithRequired(f => f.Followee)
                .WillCascadeOnDelete(false);

            HasMany(u => u.Followees) //relationship configuration
                .WithRequired(f => f.Follower)
                .WillCascadeOnDelete(false);
        }
    }
}