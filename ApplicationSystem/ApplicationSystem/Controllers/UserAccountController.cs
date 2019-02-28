using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSystem.Models;
using ApplicationSystem.ViewModels;

namespace ApplicationSystem.Controllers
{
    public class UserAccountController : Controller
    {
        ApplicationDbContext _context;

        public UserAccountController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: UserAccount
        public ActionResult Index()
        {
            return View("SignUp");
        }

        //GET: UserAccount/SignUp
        public ActionResult SignUp()
        {
            return View()
;       }

        //GET: UserAccount/SignUpCandidate
        public ActionResult SignUpCandidate()
        {
            var degrees = _context.Degrees.ToList();
            var viewModel = new CandidateFormViewModel
            {
                Degrees = degrees
            };
            return View("CandidateSignUpForm", viewModel);
        }

        //GET: UserAccount/SignUpHiringManager
        public ActionResult SignUpHiringManager()
        {
           
            return View("HiringManagerSignUpForm");
        }

        //GET: UserAccount/SignIn
        public ActionResult SignIn()
        {
            return View()
;
        }

        //GET: UserAccount/SignInCandidate
        public ActionResult SignInCandidate()
        {
            
            return View("CandidateSignInForm");
        }

        //GET: UserAccount/SignInHiringManager
        public ActionResult SignInHiringManager()
        {

            return View("HiringManagerSignInForm");
        }


        // log off and clear the session
        public ActionResult LogOff()
        {
            Session["logined"] = false;
            Session["id"] = 0;
            Session["userName"] = null;
            Session["userPrivilege"] = null;
            Session["viewingJob"] = null;
            
            return RedirectToAction("Index", "Home");
        }

    }
}