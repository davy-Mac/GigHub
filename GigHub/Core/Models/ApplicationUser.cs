using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GigHub.Core.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public ICollection<Following> Followers { get; set; } // Navigation property
        public ICollection<Following> Followees { get; set; }  // Navigation property
        public ICollection<UserNotification> UserNotifications { get; set; }  // Navigation property


        public ApplicationUser()
        {
            Followers = new Collection<Following>(); // Initializes the navigation property
            Followees = new Collection<Following>(); // Initializes the navigation property
            UserNotifications = new Collection<UserNotification>();  // Initializes the navigation property
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public void Notify(Notification notification) // notification method, takes a notification object
        {
            UserNotifications.Add(new UserNotification(this, notification)); // adds the user notification to the context
        }
    }
}
