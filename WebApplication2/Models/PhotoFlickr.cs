﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace WebApplication2.Models
{
    public class PhotoFlickr
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Titlul este obligatoriu")]
        [StringLength(6, ErrorMessage = "Titlul nu poate avea mai mult de 6 caractere")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Continutul pozei este obligatoriu")]
        public Image Photo { get; set; }

        [Required(ErrorMessage = "Continutul pozei este obligatoriu")]
        public string Description { get; set; }

        [DataType(DataType.DateTime, ErrorMessage = "Campul trebuie sa contina data si ora")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Categoria este obligatorie")]
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public virtual Comment Comment { get; set; }

        public IEnumerable<SelectListItem> Comments { get; set; }

        public virtual ApplicationUser User { get; set; }

 
    }
}