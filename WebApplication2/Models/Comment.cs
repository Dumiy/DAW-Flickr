using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int PhotoId { get; set; }
        [Required]
        public string Content { get; set; }


        public virtual ICollection<PhotoFlickr> PhotoFlickr { get; set; }

    }
}