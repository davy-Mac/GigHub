using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class NotificationConfiguration : EntityTypeConfiguration<Notification>// inherits from EntityTypeConfiguration
    {
        public NotificationConfiguration()
        {
            HasRequired(n => n.Gig); //property configuration Key mandatory one
        }
    }
}