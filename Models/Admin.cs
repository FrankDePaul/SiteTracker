using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System;

namespace SiteTracker.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }




        [Required]
        [EmailAddress]
        public string Email { get; set; }


        
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        

        



    }
}