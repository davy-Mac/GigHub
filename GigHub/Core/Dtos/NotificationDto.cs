using System;
using GigHub.Core.Models;

namespace GigHub.Core.Dtos
{
    public class NotificationDto
    {
        public DateTime DateTime { get; set; } // "private" so it's always valid cannot be changed once set

        public NotificationType Type { get; set; } // "private" so it's always valid cannot be changed once set

        public DateTime? OriginalDateTime { get; set; } // "private" so it's always valid cannot be changed once set

        public string OriginalVenue { get; set; } // "private" so it's always valid cannot be changed once set

        public GigDto Gig { get; set; } // "private" so it's always valid cannot be changed once set
    }
}
