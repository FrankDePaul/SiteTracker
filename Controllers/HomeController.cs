using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SiteTracker.Models;

namespace SiteTracker.Controllers
{
    public class HomeController : Controller
    {
        private YourContext _context;
        public HomeController(YourContext context) {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("AdminLogin")]
        public IActionResult AdminLogin() 
        {
            // Admin newadmin = new Admin();
            // newadmin.Password = "12345678";
            // newadmin.Email = "herb@s.com";
            // PasswordHasher<Admin> Hasher = new PasswordHasher<Admin>();
            //         newadmin.Password = Hasher.HashPassword(newadmin, newadmin.Password);


            // _context.Add(newadmin);
            // _context.SaveChanges();
                  
           
                return View("AdminLogin");
        
            
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(Admin FormData) 
        {

            if(ModelState.IsValid)
            {
                Admin LoginAdmin = new Admin();

                var admin = _context.Admins.SingleOrDefault(a => a.Email == FormData.Email); 

                if(admin !=null)
            {
                    var Hasher = new PasswordHasher<Admin>();
                    if(0 !=Hasher.VerifyHashedPassword(admin, admin.Password, FormData.Password))
                    {
                        HttpContext.Session.SetInt32("id", admin.Id);
                        return Redirect("/AdminPage");
                    }
                }
                 TempData["InvalidLogin"] = "    Incorrect email or password";
                return View("AdminLogin", FormData);
            }
            else
            {
                TempData["InvalidLogin"] = "    Incorrect email or password";
                return View("AdminLogin", FormData);
            }
            
        }



        // Routes to the Admin page so the admin can log in
        [HttpGet]
        [Route("AdminPage")]
        public IActionResult AdminPage() 
        {       
                var id = HttpContext.Session.GetInt32("id");
                if(id == null)
                {
                    return Redirect("/");
                }
         
                return View("AdminPage");
                   
        }


//*****************************************************************ADDING A SITE TO THE DATABASE****************************************

        //Routes to the page where the Admin can add sites
        [HttpGet]
        [Route("AddSitePage")]
        public IActionResult AddSitePage() 
        {
                var id = HttpContext.Session.GetInt32("id");
                if(id == null)
                {
                    return Redirect("/");
                }
         
                return View();
                   
        }

        //Validation for adding a new site to the database. If valid, site will be added to the database.
        [HttpPost]
        [Route("AddSite")]
        public IActionResult AddSite(Site FormData)
        {
            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return Redirect("/");
            }
            if(ModelState.IsValid)
            {
                Site newSite = new Site();
                Site siteNumberCheck = _context.Sites.SingleOrDefault(s=>s.Site_Number == FormData.Site_Number);
                Site siteEnbCheck = _context.Sites.SingleOrDefault(s=>s.Enb_Number == FormData.Enb_Number);
                Site siteEnb2Check = _context.Sites.SingleOrDefault(s=>s.Enb2_Number == FormData.Enb2_Number);
                Site siteFourDigit= _context.Sites.SingleOrDefault(s=>s.FourDigit_Number == FormData.FourDigit_Number);

                 bool allUnique = true;

                if(siteNumberCheck != null)
                {
                    ModelState.AddModelError("Site_Number", "Site number already exists");
                    allUnique = false;
                }
                if(siteEnbCheck != null)
                {
                    ModelState.AddModelError("Enb_Number", "ENB number already exists");
                    allUnique = false;
                }
                if(siteEnb2Check != null)
                {
                    ModelState.AddModelError("Enb2_Number", "ENB2 number already exists");
                    allUnique = false;
                }
                if(siteFourDigit != null)
                {
                    ModelState.AddModelError("FourDigit_Number", "ENB2 number already exists");
                    allUnique = false;
                }

              
                if(allUnique == true)
                {
                    newSite.Lattitude = FormData.Lattitude;
                    newSite.Longitude = FormData.Longitude;                   
                    newSite.Site_Number= FormData.Site_Number;
                    newSite.Tech_Name = FormData.Tech_Name;
                    newSite.Tech_Phone =FormData.Tech_Phone;
                    newSite.Enb_Number =FormData.Enb_Number;
                    newSite.Enb2_Number =FormData.Enb2_Number;
                    newSite.FourDigit_Number =FormData.FourDigit_Number;
                    newSite.Site_Name =FormData.Site_Name;

                    _context.Add(newSite);
                    _context.SaveChanges();
                    Site siteAdded = _context.Sites.Last();
                    
                    return Redirect("SiteAdded");

                }
                
                return View("AddSitePage", FormData);

            }
            return View("AddSitePage", FormData);
        }

        //Confirmation showing the site has been added.
        [HttpGet]
        [Route("SiteAdded")]
        public IActionResult SiteAdded()
        {
            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return Redirect("/");
            }

            Site siteAdded = _context.Sites.Last();

            return View(siteAdded);
        }



//*****************************************************************EDITING SITE INFORMATION****************************************



        //Routes to the page where the Admin can edit site informaiton. Shows a list of sites to edit
        [HttpGet]
        [Route("EditSitePage")]
        public IActionResult EditSitePage() 
        
        {
            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return Redirect("/");
            }
            List<Site> AllSites = _context.Sites.ToList();
            ViewBag.AllSites = AllSites;
         
            return View();
                   
        }

        //After clicking on site to edit link from EditSitePage, will render a form to edit the information
        [HttpGet]
        [Route("EditSite/{site_id}")]
        public IActionResult EditSite(int site_id)
        {


            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return Redirect("/");
            }            

            Site editSite = _context.Sites.SingleOrDefault(site=>site.Id == site_id);

            if(editSite == null)
            {
                return Redirect("/EditSitePage");
            }


            ViewBag.EditSite = editSite;
            HttpContext.Session.SetInt32("site_id", site_id);
            return View();
        }

        //Form post to update site iformation
        [HttpPost]
        [Route("UpdateSite")]
        public IActionResult UpdateSite(Site FormData)
        {
            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return Redirect("/");
            }

            Site edited_Site = _context.Sites.SingleOrDefault(site=>site.Id == HttpContext.Session.GetInt32("site_id"));
      
            int site_id = edited_Site.Id;
            if(ModelState.IsValid)
            {

                Site siteNumberCheck = _context.Sites.SingleOrDefault(s=>s.Site_Number == FormData.Site_Number);
                Site siteEnbCheck = _context.Sites.SingleOrDefault(s=>s.Enb_Number == FormData.Enb_Number);
                Site siteEnb2Check = _context.Sites.SingleOrDefault(s=>s.Enb2_Number == FormData.Enb2_Number);
                Site siteFourDigit= _context.Sites.SingleOrDefault(s=>s.FourDigit_Number == FormData.FourDigit_Number);

                 bool allUnique = true;

                if(siteNumberCheck !=null && edited_Site.Site_Number != FormData.Site_Number)
                {
                    allUnique = false;
                    TempData["Site_Number"]= "Site number already exists";
                }
                if(siteEnbCheck !=null && edited_Site.Enb_Number != FormData.Enb_Number)
                {
                    allUnique = false;
                    TempData["Enb_Number"]= "Enb Number already exists";
                }
                if(siteEnb2Check !=null && edited_Site.Enb2_Number != FormData.Enb2_Number)
                {
                    allUnique = false;
                    TempData["Enb2_Number"]= "Enb2 Number already exists";
                }
                if(siteEnb2Check !=null && edited_Site.FourDigit_Number != FormData.FourDigit_Number)
                {
                    allUnique = false;
                    TempData["FourDigit_Number"]= "Four Digit Number already exists";
                }

                if(allUnique == true)
                {
                        edited_Site.FourDigit_Number = FormData.FourDigit_Number;
                        edited_Site.Enb_Number = FormData.Enb_Number;
                        edited_Site.Enb2_Number = FormData.Enb2_Number;
                        edited_Site.Site_Name = FormData.Site_Name;
                        edited_Site.Tech_Name = FormData.Tech_Name;
                        edited_Site.Tech_Phone = FormData.Tech_Phone;
                        edited_Site.Site_Number = (int)FormData.Site_Number;
                        edited_Site.Longitude = FormData.Longitude;
                        edited_Site.Lattitude = FormData.Lattitude;
                        _context.Update(edited_Site);
                        _context.SaveChanges();
                        return Redirect("SiteEdited");                               
                }
                return Redirect($"EditSite/{site_id}");
                                        
            }
            TempData["Error"]= "Please enter valid information";
            return Redirect($"EditSite/{site_id}");

            
        }


        //Confirmation page that the site has been edit.
        [HttpGet]
        [Route("SiteEdited")]
        public IActionResult SiteEdited()
        {

            var id = HttpContext.Session.GetInt32("id");
            if(id == null)
            {
                return Redirect("/");
            }

            Site sitedEdited = _context.Sites.SingleOrDefault(site=>site.Id == HttpContext.Session.GetInt32("site_id"));
            ViewBag.SiteEdited = sitedEdited;
            return View();
        }


// **********************************************SEARCHING FOR SITES**************************************************


        [HttpPost]
        [Route("Search")]
        public IActionResult Search(Search FormData) 
        {
            
            System.Console.WriteLine("**************** type: " + FormData.Site_Numbers.GetType());
    
            if(ModelState.IsValid)
            {
                
                Site siteNumber = _context.Sites.SingleOrDefault(site=>site.Site_Number == FormData.Site_Numbers);
                if(siteNumber != null)
                {
                    ViewBag.DisplaySite = siteNumber;
                    return View("DisplaySite");
                }

                Site enbNumber = _context.Sites.SingleOrDefault(site=>site.Enb_Number == FormData.Site_Numbers);
                if(enbNumber != null)
                {
                    ViewBag.DisplaySite = enbNumber;
                    return View("DisplaySite");
                }
                Site enb2Number = _context.Sites.SingleOrDefault(site=>site.Enb2_Number == FormData.Site_Numbers);
                if(enb2Number != null)
                {
                    ViewBag.DisplaySite = enb2Number;
                    return View("DisplaySite");
                }


                TempData["Site_Number"]="Site could not be found";
                return View("Index");
            }
            return View("Index",FormData);
            
        }

        [HttpPost]
        [Route("Search2")]
        public IActionResult Search2(Search FormData)
        {
            if(FormData.SiteNumber >= 1000 &&  FormData.SiteNumber <= 9999)
            {
                Site siteFourDig = _context.Sites.SingleOrDefault(site=>site.FourDigit_Number == FormData.SiteNumber);
                if(siteFourDig != null)
                {
                    ViewBag.DisplaySite = siteFourDig;
                    return View("DisplaySite");
                }
                TempData["Site_Number"]="Site could not be found";
                return View("Index");               
            }



            if(FormData.SiteNumber >= 100000 && FormData.SiteNumber <= 999999)
            {
                Site siteNumber = _context.Sites.SingleOrDefault(site=>site.Site_Number == FormData.SiteNumber);
                if(siteNumber != null)
                {
                    ViewBag.DisplaySite = siteNumber;
                    return View("DisplaySite");
                }

                Site enbNumber = _context.Sites.SingleOrDefault(site=>site.Enb_Number == FormData.SiteNumber);
                if(enbNumber != null)
                {
                    ViewBag.DisplaySite = enbNumber;
                    return View("DisplaySite");
                }
                Site enb2Number = _context.Sites.SingleOrDefault(site=>site.Enb2_Number == FormData.SiteNumber);
                if(enb2Number != null)
                {
                    ViewBag.DisplaySite = enb2Number;
                    return View("DisplaySite");
                } 
                TempData["Site_Number"]="Site could not be found";
                return View("Index");           
            }
            TempData["Site_Number"]="Please enter a 4 or 6 digit site number";
            return View("Index"); 

            
            
        }





        [HttpGet]
        [Route("SiteList/{sort}")]
        public IActionResult SiteList(int sort)
        {
            List<Site> AllSites = _context.Sites.ToList();
            if(sort == 1)
            {
                List<Site> SitesByNumber = _context.Sites.OrderBy(site=>site.Site_Number).ToList();
                ViewBag.AllSites =SitesByNumber;           
                return View();
            }
            if(sort == 2)
            {
                List<Site> SitesByEnb = _context.Sites.OrderBy(site=>site.Enb_Number).ToList();
                ViewBag.AllSites =SitesByEnb ;           
                return View();
            }
            if(sort == 3)
            {
                List<Site> SitesByEnb2 = _context.Sites.OrderBy(site=>site.Enb2_Number).ToList();
                ViewBag.AllSites =SitesByEnb2 ;           
                return View();
            }
            if(sort == 4)
            {
                List<Site> SitesByFourDig = _context.Sites.OrderBy(site=>site.FourDigit_Number).ToList();
                ViewBag.AllSites =SitesByFourDig ;           
                return View();
            }
            if(sort == 5)
            {
                List<Site> SitesByName = _context.Sites.OrderBy(site=>site.Site_Name).ToList();
                ViewBag.AllSites =SitesByName ;           
                return View();
            }






            ViewBag.AllSites =AllSites; 
            return View();

        }

        [HttpGet]
        [Route("ShowSiteInfo/{site_id}")]
        public IActionResult ShowSiteInfo(int site_id)
        {
            Site siteSearched = _context.Sites.SingleOrDefault(site=>site.Id == site_id);
            ViewBag.DisplaySite = siteSearched;
            return View("DisplaySite");
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return Redirect("/");
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
