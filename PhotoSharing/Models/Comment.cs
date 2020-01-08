using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhotoSharing.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public DateTime Timestamp { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Photo Photo { get; set; }
    }
}