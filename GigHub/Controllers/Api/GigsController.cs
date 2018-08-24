using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class GigsController : ApiController
    {
        private ApplicationDbContext _context;

        public GigsController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpDelete]
        public IHttpActionResult Cancel(int id)  // when this action is called the IsCanceled boolean is set to true
        {
            var userId = User.Identity.GetUserId(); // to get the user Id
            //var gig = _context.Gigs.Single(g => g.Id == id && g.ArtistId == userId); // queries the db without eager loading 
            var gig = _context.Gigs
                .Include(g => g.Attendances.Select(a => a.Attendee)) // eager loads the attendee
                .Single(g => g.Id == id && g.ArtistId == userId);

            if (gig.IsCanceled)
                return NotFound();

            gig.Cancel();



            // OLD WAY OF CREATING A NEW NOTIFICATION
            //var notification = new Notification // creates a new object Notification
            //{
            //    DateTime = DateTime.Now,
            //    Gig = gig, // the local gig variable
            //    Type = NotificationType.GigCanceled // Type of notification GigCanceled
            //};

            //var notification = new Notification(NotificationType.GigCanceled, gig); // creates a new object Notification



            //NOT NECESSARY AS WE GET THE ATTENDEES FROM THE var gig THAT EAGER LOADS THEM
            //var attendees = _context.Attendances //Gets the attendees form the Database for a specific Gig Notification
            //    .Where(a => a.GigId == gig.Id)
            //    .Select(a => a.Attendee) // selects the ApplicationUser
            //    .ToList();

            //OLD WAY OF ITERATING THRU THE ATTENDEES
            //foreach (var attendee in attendees)
            //{
            //    var userNotification = new UserNotification
            //    {
            //        User = attendee,
            //        Notification = notification
            //    };
            //}

            _context.SaveChanges();  // saves the notification to the DB

            return Ok();
        }
    }
}
