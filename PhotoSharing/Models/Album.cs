using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharing.Models
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public virtual ApplicationUser Owner { get; set; }
    }
}