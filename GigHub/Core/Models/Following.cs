using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GigHub.Core.Models
{
    public class Following
    {
        //[Key]
        //[Column(Order = 1)]// not necessary here, defined in Entity Configurations
        public string FollowerId { get; set; }

        //[Key]
        //[Column(Order = 2)] // not necessary here, defined in Entity Configurations
        public string FolloweeId { get; set; }

        public ApplicationUser Follower { get; set; }
        public ApplicationUser Followee { get; set; }
    }
}
