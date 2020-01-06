using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Albums
    {
        [Key]
        public int AlbumId { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required(ErrorMessage = "Numele albumului este obligatoriu")]
        public string AlbumName { get; set; }


        public virtual ICollection<PhotoFlickr> Photos { get; set; }
    }
}     