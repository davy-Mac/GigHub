using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace GigHub.Core.Models
{
    public class Gig
    {
        public int Id { get; set; }

        public bool IsCanceled { get; private set; }

        public ApplicationUser Artist { get; set; }

        //[Required] // not necessary here anymore been defined in Entity Configurations Folder
        public string ArtistId { get; set; }

        public DateTime DateTime { get; set; }

        //[Required]
        //[StringLength(255)] // not necessary here anymore been defined in Entity Configurations Folder
        public string Venue { get; set; }

        public Genre Genre { get; set; }

        //[Required]
        public byte GenreId { get; set; }

        public ICollection<Attendance> Attendances { get; private set; }

        public Gig()
        {
            Attendances = new Collection<Attendance>();
        }

        public void Cancel()
        {
            IsCanceled = true;

            var notification = Notification.GigCanceled(this); // Creates a new notification

            foreach (var attendee in Attendances.Select(a => a.Attendee))  // Loops through each attendee to create a notification for every attendee
            {
                attendee.Notify(notification); // calls the notify method
            }
        }

        public void Modify(DateTime dateTime, string venue, byte genre) // Modify/Update a Gig
        {
            var notification = Notification.GigUpdated(this, DateTime, Venue);
            //notification.OriginalDateTime = DateTime;
            //notification.OriginalVenue = Venue;

            Venue = venue; // updates the field
            DateTime = dateTime;  // updates the field
            GenreId = genre;  // updates the field

            foreach (var attendee in Attendances.Select(a => a.Attendee)) // loops thru the attendees to notify them
                attendee.Notify(notification);
        }
    }
}
