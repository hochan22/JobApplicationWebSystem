using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSystem.Models;
using ApplicationSystem.ViewModels;

namespace ApplicationSystem.Controllers
{
    public class HiringManagersController : Controller
    {
        ApplicationDbContext _context;

        public HiringManagersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: HiringManagers
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Jobs");
        }

        [HttpPost]
        public ActionResult Save(HiringManager hiringManager)
        {
            var allHM = _context.HiringManagers.ToList();
            for (int i = 0; i < allHM.Count(); i++)
            {
                if (allHM.ElementAt(i).Email.Equals(hiringManager.Email) == true)
                {
                    return View("EmailExisted");
                }
            }
            
            // check the code for hiring manager
            if (hiringManager.Code == Codes.secretCode)
            {
                hiringManager.Privilege = 2;
                _context.HiringManagers.Add(hiringManager);
                
                _context.SaveChanges();
                Session["logined"] = true;
                
                Session["userName"] = hiringManager.Name;
                Session["userPrivilege"] = hiringManager.Privilege;

                var hmid = _context.HiringManagers.ToList().Last().Id;
                Session["id"] = hmid;
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                return RedirectToAction("SignUpHiringManager", "UserAccount");
            }
            
        }

        // login check for hiring manager
        public ActionResult Check(HiringManager hiringManager)
        {
            var hiringManagers = _context.HiringManagers.ToList();
            var inputEmail = hiringManager.Email;
            var inputPassword = hiringManager.Password;
            foreach (var hm in hiringManagers)
            {
                if (hm.Email.Equals(inputEmail) && hm.Password.Equals(inputPassword))
                {
                    Session["logined"] = true;
                    Session["id"] = hm.Id;
                    Session["userName"] = hm.Name;
                    Session["userPrivilege"] = hm.Privilege;
                    return RedirectToAction("Index", "Home");
                    
                }
               
            }
            return RedirectToAction("SignIn", "UserAccount");
        }
    }
}