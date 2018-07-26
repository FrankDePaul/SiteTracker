using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SiteTracker.Models
{

    public class Search
    {



        [Required]
        [Range (100000,999999, ErrorMessage = "Site number must be 6 digits")]
        public int Site_Numbers {get; set;}

   
        public int SiteNumber {get; set;}


       
        
           
        
        

        

        
    }
}