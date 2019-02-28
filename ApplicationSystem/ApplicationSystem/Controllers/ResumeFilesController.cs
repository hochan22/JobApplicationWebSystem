using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSystem.Models;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace ApplicationSystem.Controllers
{
    public class ResumeFilesController : Controller
    {
        ApplicationDbContext _context;

        public ResumeFilesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        // GET: ResumeFiles
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }


        
        public ActionResult FileUpload()
        {
            if (Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 1 && Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            var id = Int32.Parse(Session["viewingJob"].ToString());
            var jobInDb = _context.Jobs.Single(j => j.Id == id);
            // check whether the candidate has applied before
            var list = jobInDb.CandidatesList;
            if (list != null)
            {
                string[] candidates = list.Split('$');
                for (int i = 1; i < candidates.Count(); i++)
                {
                    string[] items = candidates.ElementAt(i).Split('_');
                    int existId = Int32.Parse(items.ElementAt(0));
                    if (existId == Int32.Parse(Session["id"].ToString())) 
                    {
                        return View("CandidateAppliedBefore");
                    }
                }
            }
            return View();
        }

        [HttpPost]
        public ActionResult FileUpload(HttpPostedFileBase files)
        {
            string FileExt = Path.GetExtension(files.FileName).ToUpper();
            if(FileExt == ".PDF")
            {
                Stream stream = files.InputStream;
                BinaryReader binaryReader = new BinaryReader(stream);
                Byte[] FileDet = binaryReader.ReadBytes((Int32)stream.Length);

                PdfFile file = new Models.PdfFile();
                file.FileName = files.FileName;
                file.FileContent = FileDet;
                _context.PdfFiles.Add(file);
                _context.SaveChanges();
                var fileId = _context.PdfFiles.ToList().Last().Id;
                var ID = Int32.Parse(Session["viewingJob"].ToString());
                return RedirectToAction("Apply", "Jobs", new { id = ID, fid = fileId });
            }
            else
            {
                ViewBag.FileStatus = "Invalid file format";
                return View("InvalidFileWarning");
            }
        }
    }
}