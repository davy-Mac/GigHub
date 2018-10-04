using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class Attendance
    {
        public Gig Gig { get; set; }
        public ApplicationUser Attendee { get; set; }

        //[Key]
        //[Column(Order = 1)] // not necessary here, defined in Entity Configurations
        public int GigId { get; set; }

        //[Key]
        //[Column(Order = 2)] // not necessary here, defined in Entity Configurations
        public string AttendeeId { get; set; }
    }
}
