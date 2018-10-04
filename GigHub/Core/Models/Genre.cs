using System.ComponentModel.DataAnnotations;

namespace GigHub.Core.Models
{
    public class Genre
    {
        public byte Id { get; set; }

        //[Required]
        //[StringLength(255)] // not necessary here, defined in Entity Configurations
        public string Name { get; set; }
    }
}
