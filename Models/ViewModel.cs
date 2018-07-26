using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace SiteTracker.Models
{

    public class ViewModel
    {


        public Site site {get; set;}
        public List<Site>AllSites {get;set;}

       
       
       
    }
    



}