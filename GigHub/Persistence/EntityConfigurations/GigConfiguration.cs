using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class GigConfiguration : EntityTypeConfiguration<Gig>// inherits from EntityTypeConfiguration
    {
        public GigConfiguration()
        {
            Property(g => g.ArtistId) //property configuration 
                .IsRequired();

            Property(g => g.GenreId) //property configuration 
                .IsRequired();

            Property(g => g.Venue) //property configuration 
                .IsRequired()
                .HasMaxLength(255);

            HasMany(g => g.Attendances) //relationship configuration in here because Gig is parent or stronger than Attendance
                .WithRequired(a => a.Gig)
                .WillCascadeOnDelete(false);
        }

    }
}