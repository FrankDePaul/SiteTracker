using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SiteTracker.Models
{

    public class Site
    {
        [Key]
       
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter between 2 and 255 characters")]
        [MinLength(2),MaxLength(255)]
        public string Site_Name {get; set;}

        [Required]
        public string Lattitude {get; set;}
        [Required]
        public string Longitude {get; set;}


        [Required]
        [Range (100000,999999, ErrorMessage = "Site number must be 6 digits")]
        public int Site_Number {get; set;}

        [Required]
        [Range (100000,999999, ErrorMessage = "Site number must be 6 digits")]
        public int Enb_Number {get; set;}

        [Required]
        [Range (100000,999999, ErrorMessage = "Site number must be 6 digits")]
        public int Enb2_Number {get; set;}

        [Required]
        [Range (1000,9999, ErrorMessage = "Site number must be 4 digits")]
        public int FourDigit_Number {get; set;}





        [Required]
        [MinLength(2),MaxLength(255)]
        public string Tech_Name {get; set;}

        [Required]
        [MinLength(10),MaxLength(10)]
        public string Tech_Phone {get; set;}
       
        
           
        
        
    // https://www.google.com/maps/@36.1518704,-95.9973116
       
        
        public Site(){
            
            Lattitude = "37.0457471";
            Longitude = "-94.4445743";
            
        }
        

        
    }
}