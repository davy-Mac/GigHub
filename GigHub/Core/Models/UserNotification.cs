using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class UserNotification
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; private set; } //"private" so it's always valid cannot be changed once set

        [Key]
        [Column(Order = 2)]
        public int NotificationId { get; private set; } //"private" so it's always valid cannot be changed once set

        public ApplicationUser User { get; private set; } // navigation property, "private" so it's always valid cannot be changed once set

        public Notification Notification { get; private set; }  // navigation property, "private" so it's always valid cannot be changed once set

        public bool IsRead { get; private set; }

        protected UserNotification() // default constructor for EF
        {
        }

        public UserNotification(ApplicationUser user, Notification notification) // custom constructor consists of user & notification
        {
            if (user == null)
                throw new ArgumentException("user");

            if (notification == null)
                throw new ArgumentException("notification");

            User = user;
            Notification = notification;
        }

        public void Read() // sets the IsRead state to Read, true
        {
            IsRead = true;
        }
    }
}
