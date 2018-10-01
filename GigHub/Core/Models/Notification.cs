using System;
using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Notification
    {
        public int Id { get; private set; } // "private" so it's always valid cannot be changed once set
        public DateTime DateTime { get; private set; } // "private" so it's always valid cannot be changed once set
        public NotificationType Type { get; private set; } // "private" so it's always valid cannot be changed once set
        public DateTime? OriginalDateTime { get; private set; } // "private" so it's always valid cannot be changed once set
        public string OriginalVenue { get; private set; } // "private" so it's always valid cannot be changed once set

        [Required]
        public Gig Gig { get; private set; } // "private" so it's always valid cannot be changed once set

        protected Notification() // default constructor for EF
        {
        }

        private Notification(NotificationType type, Gig gig) // constructor that consist of type & gig
        {
            if (gig == null)
                throw new ArgumentException("gig");

            Type = type;
            Gig = gig;
            DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(NotificationType.GigCreated, gig);
        }
        public static Notification GigUpdated(Gig newGig, DateTime originalDateTime, string originalVenue)
        {
            var notification = new Notification(NotificationType.GigUpdated, newGig);
            notification.OriginalDateTime = originalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification GigCanceled(Gig gig)
        {
            return new Notification(NotificationType.GigCanceled, gig);
        }

    }
}
