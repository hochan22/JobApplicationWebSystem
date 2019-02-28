using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSystem.Models;
using ApplicationSystem.ViewModels;
using System.Data.Entity;
using System.IO;

namespace ApplicationSystem.Controllers
{
    public class CandidatesController : Controller
    {
        ApplicationDbContext _context;

        public CandidatesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // Check User Privilege, only privilege == 2 can see candidate list
        // GET: Candidates
        public ActionResult Index()
        {
            if(Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            var candidates = _context.Candidates.ToList();
            return View(candidates);
        }


        //New a candidate
        public ActionResult New()
        {
            var degrees = _context.Degrees.ToList();
            var viewModel = new CandidateFormViewModel
            {
                Degrees = degrees
            };
            return View("CandidateForm", viewModel);
        }


        // Sign up a new candidate and use this save method
        // Edit an existed candidate information and use this save method
        [HttpPost]
        public ActionResult Save(Candidate candidate)
        {
            var allCan = _context.Candidates.ToList();
            for(int i = 0; i < allCan.Count(); i++)
            {
                if(allCan.ElementAt(i).Email.Equals(candidate.Email) == true)    // Check duplicate email
                {
                    return View("EmailExisted");
                }
            }
            // valid email address
            if (candidate.Id == 0)    //new candidate
            {
                candidate.Privilege = 1;    // candidate privilege
                candidate.Notes = "";    // initial not null
                _context.Candidates.Add(candidate);
                // set session
                Session["logined"] = true;
                
                Session["userName"] = candidate.Name;
                Session["userPrivilege"] = candidate.Privilege;
                Session["viewingJob"] = 0;
                _context.SaveChanges();
                var canId = _context.Candidates.ToList().Last().Id;
                Session["id"] = canId;

            }
            else    // edit the candidate information
            {
                var candidateInDb = _context.Candidates.Single(c => c.Id == candidate.Id);
                candidateInDb.DegreeId = candidate.DegreeId;
                candidateInDb.Major = candidate.Major;
                candidateInDb.University = candidate.University;
                _context.SaveChanges();
            }
            
            
            return RedirectToAction("Index", "Home");
        }


        // Sign in an existed candidate and use this check method to check email and password
        [HttpPost]
        public ActionResult Check(Candidate candidate)
        {
            var candidates = _context.Candidates.ToList();
            var inputEmail = candidate.Email;
            var inputPassword = candidate.Password;
            foreach(var c in candidates)
            {
                if(c.Email.Equals(inputEmail) == true && c.Password.Equals(inputPassword) == true)
                {
                    Session["logined"] = true;
                    Session["id"] = c.Id;
                    Session["userName"] = c.Name;
                    Session["userPrivilege"] = c.Privilege;
                    Session["viewingJob"] = 0;
                    return RedirectToAction("Index", "Home");
                    
                }
            }
            return RedirectToAction("SignIn", "UserAccount");
        }


        // Show candidate's information, hiring manager can see the notes
        //Candidates/Details/Id
        public ActionResult Details(int id)
        {
            Candidate candidate = _context.Candidates.Include(c => c.Degree).SingleOrDefault(c => c.Id == id);
            if(Session["userPrivilege"] == null)    
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            else if (Int32.Parse(Session["userPrivilege"].ToString()) == 1)
            {
                return View("DetailsForCandidate", candidate);
            }
            else if (Int32.Parse(Session["userPrivilege"].ToString()) == 2)
            {
                //  privilege == 2 can see notes of candidates
                var notes = candidate.Notes;
                if (notes.Length != 0)    // the candidate has note
                {
                    List<string> noteList = new List<string>();
                    string[] splitedNotes = notes.Split('$');
                    for (int i = 1; i < splitedNotes.Length; i++)
                    {
                        noteList.Add(splitedNotes.ElementAt(i));
                    }
                    CandidatesDetailViewModel viewModel = new CandidatesDetailViewModel
                    {
                        Notes = noteList,
                        Candidate = candidate

                    };
                    return View("DetailsForHiringManager", viewModel);
                }
                else   // the candidate does not have note
                {
                    CandidatesDetailViewModel viewModel = new CandidatesDetailViewModel
                    {
                        Notes = null,
                        Candidate = candidate
                    };
                    return View("DetailsForHiringManager", viewModel);
                }
            }
            else    
            {
                return RedirectToAction("SignIn", "UserAccount");
            }

        }


        // Hiring Manager can add notes to any candidates
        // note form: "$xxxx$xxxxxx$xxx"
        public ActionResult AddNotes(int id)
        {
            if (Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            Candidate candidate = _context.Candidates.Single(c => c.Id == id);
            NotesViewModel viewModel = new NotesViewModel
            {
                Id = candidate.Id
            };
            return View("NoteForm", viewModel);
        }


        // After using AddNotes, use SaveNote to save note to the candidate
        // note form: "$xxxx$xxxxxx$xxx"
        [HttpPost]
        public ActionResult SaveNote(NotesViewModel candidate)
        {
            var ID = candidate.Id;
            // obtain the candidate
            Candidate candidateInDb = _context.Candidates.Single(c => c.Id == ID);
            // add and save the new note
            candidateInDb.Notes = candidateInDb.Notes + "$" + candidate.Note;
            _context.SaveChanges();
            
            return RedirectToAction("Details", "Candidates", new { id = ID });
        }



        // Hiring Manager can see the resume of any candidates
        public ActionResult DisplayResumePdf(int rid)
        {
            if (Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            byte[] byteArray = _context.PdfFiles.Single(r => r.Id == rid).FileContent;
            MemoryStream pdfStream = new MemoryStream();
            pdfStream.Write(byteArray, 0, byteArray.Length);
            pdfStream.Position = 0;
            return new FileStreamResult(pdfStream, "application/pdf");
        }


        // signed candidate can edit the information
        public ActionResult Edit(int id)
        {
            if ((Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 1) && Int32.Parse(Session["id"].ToString()) != id)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            Candidate candidateInDb = _context.Candidates.Single(c => c.Id == id);
            var degrees = _context.Degrees.ToList();
            CandidateFormViewModel viewModel = new CandidateFormViewModel
            {
                Candidate = candidateInDb,
                Degrees = degrees
            };
            return View("CandidateForm", viewModel);
        }


        // sppliedjobList form: "$11_1$12_2"("$jobId_StatusId")
        public ActionResult ShowAppliedJobs(int id)
        {
            if(Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 1)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            //int id = Int32.Parse(Session["id"].ToString());
            Candidate candidateInDb = _context.Candidates.Single(c => c.Id == id);
            var jobList = candidateInDb.AppliedJobsList;
            if(jobList != null)
            {
                string[] jobs = jobList.Split('$');
                List<string> appliedJobs = new List<string>();
                List<string> statuses = new List<string>();
                List<int> appliedJobsId = new List<int>();
                // get all applied jobs
                for(int i = 1; i < jobs.Count(); i++)
                {
                    string[] items = jobs.ElementAt(i).Split('_');
                    int jobId = Int32.Parse(items.ElementAt(0));
                    appliedJobsId.Add(jobId);
                    string jobName = _context.Jobs.Single(j => j.Id == jobId).Name;
                    appliedJobs.Add(jobName);
                    int statusId = Int32.Parse(items.ElementAt(1));
                    switch (statusId)
                    {
                        case 0:
                            statuses.Add("No Apply");
                            break;
                        case 1:
                            statuses.Add("Pending");
                            break;
                        case 2:
                            statuses.Add("Reviewed");
                            break;
                        case 3:
                            statuses.Add("Phone Interviewed");
                            break;
                        case 4:
                            statuses.Add("Onsite Interviewed");
                            break;
                        case 5:
                            statuses.Add("Offer Extended");
                            break;
                        case 6:
                            statuses.Add("Hired");
                            break;
                        case -1:
                            statuses.Add("Rejected");
                            break;
                        default:
                            statuses.Add("Contact the Recuiter");
                            break;
                    }
                }

                CandidateAppliedJobsViewModel viewModel = new CandidateAppliedJobsViewModel
                {
                    AppliedJobs = appliedJobs,
                    AppliedJobsId = appliedJobsId,
                    Statuses = statuses
                };
                return View("ShowAppliedJobs", viewModel);
            }
            else
            {
                CandidateAppliedJobsViewModel nullViewModel = new CandidateAppliedJobsViewModel
                {
                    AppliedJobs = null,
                    AppliedJobsId = null,
                    Statuses = null
                };
                return View("ShowAppliedJobs", nullViewModel);
            }
        }

        // candidateList form: "$10_12_1"("$candidateId_resumeId_statusId")
        public ActionResult WithdrawJob(int id)
        {
            if (Session["userPrivilege"] == null || Session["id"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 1 || Int32.Parse(Session["id"].ToString()) == 0)
            {
                return RedirectToAction("SignIn", "UserAccount");

            }
            else
            {
                var jid = Int32.Parse(Session["viewingJob"].ToString());
                // change candidateList for this job
                Job jobInDb = _context.Jobs.Single(j => j.Id == jid);
                string[] cands = jobInDb.CandidatesList.Split('$');
                string newCandidatesList = "";
                int cid = Int32.Parse(Session["id"].ToString());
                for (int i = 1; i < cands.Length; i++)
                {
                    string[] items = cands.ElementAt(i).Split('_');
                    int candId = Int32.Parse(items.ElementAt(0));
                    if (candId != cid)
                    {
                        newCandidatesList += "$" + cands.ElementAt(i);
                    }
                }
                jobInDb.CandidatesList = newCandidatesList;

                //change appliedJobsList for this candidate
                Candidate candidateInDb = _context.Candidates.Single(c => c.Id == cid);
                string[] jobs = candidateInDb.AppliedJobsList.Split('$');
                string newJobsList = "";
                for (int i = 1; i < jobs.Count(); i++)
                {
                    string[] items = jobs.ElementAt(i).Split('_');
                    int jobId = Int32.Parse(items.ElementAt(0));
                    if (jobId != jid)
                    {
                        newJobsList += "$" + jobs.ElementAt(i);
                    }
                }
                candidateInDb.AppliedJobsList = newJobsList;

                _context.SaveChanges();
                return RedirectToAction("ShowAppliedJobs", new { id = cid });
            }
        }
    }
}