using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public  string userId { get; set; }
        [Required(ErrorMessage = "Please enter the image.")]
        public byte[] picture { get; set; }
        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(20, ErrorMessage = "Titlul nu poate avea mai mult de 20 caractere")]
        public string name { get; set; }
        [Required(ErrorMessage = "Categoria este obligatorie")]
        public string category { get; set; }
        [DataType(DataType.DateTime, ErrorMessage = "Campul trebuie sa contina data si ora")]
        public string date { get; set; }

       public virtual ApplicationUser User { get; set; }
    }
    public class ImageDbContext : DbContext
    {
        public ImageDbContext() : base("DBConnectionString") { }
        public DbSet<Image> Images { get; set; }
    }
    
}