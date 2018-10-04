using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using GigHub.Core.Models;

namespace GigHub.Persistence.EntityConfigurations
{
    public class UserNotificationConfiguration : EntityTypeConfiguration<UserNotification>// inherits from EntityTypeConfiguration
    {
        public UserNotificationConfiguration()
        {
            HasKey(un => new { un.UserId, un.NotificationId }); //property configuration Key

            HasRequired(n => n.User) //property configuration
                .WithMany(u => u.UserNotifications)
                .WillCascadeOnDelete(false);
        }
    }
}