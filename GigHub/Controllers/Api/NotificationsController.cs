using AutoMapper;
using GigHub.Dtos;
using GigHub.Models;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace GigHub.Controllers.Api
{
    [Authorize]
    public class NotificationsController : ApiController
    {

        private ApplicationDbContext _context;

        public NotificationsController()
        {
            _context = new ApplicationDbContext();
        }

        public IEnumerable<NotificationDto> GetNewNotifications() // action to fetch the notifications form the DB
        {
            var userId = User.Identity.GetUserId();
            var notifications = _context.UserNotifications // to get the notifications for the current logged in user
                .Where(un => un.UserId == userId && !un.IsRead)
                .Select(un => un.Notification)
                .Include(n => n.Gig.Artist) // Eager loads the artist name of the artist
                .ToList();



            // the next line replaces the lambda expression in the manual way of mapping objects with a references to the Map method of the Mapper class
            return notifications.Select(Mapper.Map<Notification, NotificationDto>); // passing the reference to the "Map" method

            // MANUAL WAY OF MAPPING OBJECTS 
            //return notifications.Select(n => new NotificationDto() // mapping out the gig objects manually
            //{
            //    DateTime = n.DateTime, // mapping out the gig objects
            //    Gig = new GigDto // mapping out the gig objects
            //    {
            //        Artist = new UserDto
            //        {
            //            Id = n.Gig.Artist.Id,
            //            Name = n.Gig.Artist.Name
            //        },
            //        DateTime = n.Gig.DateTime,
            //        Id = n.Gig.Id,
            //        IsCanceled = n.Gig.IsCanceled,
            //        Venue = n.Gig.Venue
            //    },
            //    OriginalDateTime = n.OriginalDateTime,
            //    OriginalVenue = n.OriginalVenue,
            //    Type = n.Type
            //});
        }

        [HttpPost]
        public IHttpActionResult MarkAsRead() // method to mark notifications as read
        {
            var userId = User.Identity.GetUserId(); // gets the current logged in user 
            var notifications = _context.UserNotifications //gets all the notifications of a particular user
                .Where(un => un.UserId == userId && !un.IsRead) // filters the notifications by the logged in user and not Is NOT Read
                .ToList(); // immediately executes the query 

            notifications.ForEach(n=>n.Read()); // iterates over notifications to set them as read

            _context.SaveChanges();

            return Ok();
        }
    }
}
