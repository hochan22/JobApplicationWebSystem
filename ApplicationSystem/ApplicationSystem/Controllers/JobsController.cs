using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSystem.Models;
using ApplicationSystem.ViewModels;
using System.Data.Entity;
using System.Text;

namespace ApplicationSystem.Controllers
{
    public class JobsController : Controller
    {
        ApplicationDbContext _context;

        public JobsController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Jobs
        public ActionResult Index(string sortOrder, string searchString)
        {
            // obtain the sorting string
            ViewBag.NameSortParm = string.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.LocationSortParm = sortOrder == "Location" ? "Location_desc" : "Location";
            
            var jobs = from j in _context.Jobs
                       select j;
            if (!string.IsNullOrEmpty(searchString))
            {
                jobs = jobs.Where(j => j.Name.Contains(searchString));
            }
            switch(sortOrder)
            {
                case "Name_desc":
                    jobs = jobs.OrderByDescending(j => j.Name);
                    break;
                case "Location":
                    jobs = jobs.OrderBy(j => j.Location);
                    break;
                case "Location_desc":
                    jobs = jobs.OrderByDescending(j => j.Location);
                    break;
                default:
                    jobs = jobs.OrderBy(j => j.Name);
                    break;
            }
            Session["viewingJob"] = 0;
            // different privilege goes to different views
            if(Session["userPrivilege"] == null)
            {
                return View("IndexForGuest", jobs.ToList());
            }
            else if (Int32.Parse(Session["userPrivilege"].ToString()) == 1)
            {
                return View("IndexForCandidate", jobs.ToList());
            }
            else if (Int32.Parse(Session["userPrivilege"].ToString()) == 2)
            {
                return View("IndexForHiringManager", jobs.ToList());
            }
            else
            {
                return View("IndexForGuest", jobs.ToList());
            }
            
        }

       
        // Hiring Manager can add new job
        //POST: Jobs/New
        public ActionResult New()
        {
            if (Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            var degrees = _context.Degrees.ToList();
            var viewModel = new JobFormViewModel
            {
                Degrees = degrees
            };
            return View("JobForm", viewModel);
            
        }

        [HttpPost]
        public ActionResult Save(Job job)
        {
            if (job.Id == 0)    // New job
            {
                _context.Jobs.Add(job);
            }
            else    // edit existed job
            {
                var jobInDb = _context.Jobs.Single(j => j.Id == job.Id);
                jobInDb.DegreeId = job.DegreeId;
                jobInDb.Discription = job.Discription;
                jobInDb.DurationInMonths = job.DurationInMonths;
                jobInDb.Location = job.Location;
                jobInDb.Name = job.Name;
            }
            _context.SaveChanges();
            return RedirectToAction("Index", "Jobs");
        }

        //GET: Jobs/Details/Id
        // candidateList form: "$candidateId_resumeId_statusId"
        public ViewResult Details(int id)
        {
            var job = _context.Jobs.Include(j => j.Degree).SingleOrDefault(j => j.Id == id);
            Session["viewingJob"] = id;
            if(Session["userPrivilege"] == null)
            {
                return View("DetailsForGuest", job);
            }
            if(Int32.Parse(Session["userPrivilege"].ToString()) == 1)
            {
                var list = _context.Jobs.Single(j => j.Id == id).CandidatesList;
                if(list != null)   // if any candidate aply this job
                {
                    string[] cs = list.Split('$');

                    for (int i = 1; i < cs.Length; i++)
                    {
                        string[] items = cs.ElementAt(i).Split('_');
                        var cid = Int32.Parse(items.ElementAt(0));
                        if (Int32.Parse(Session["id"].ToString()) == cid)
                        {
                            var statusId = Int32.Parse(items.ElementAt(2));
                            string status = "";
                            switch (statusId)
                            {
                                case 0:
                                    status = "No Apply";
                                    break;
                                case 1:
                                    status = "Pending";
                                    break;
                                case 2:
                                    status = "Reviewed";
                                    break;
                                case 3:
                                    status = "Phone Interviewed";
                                    break;
                                case 4:
                                    status = "Onsite Interviewed";
                                    break;
                                case 5:
                                    status = "Offer Extended";
                                    break;
                                case 6:
                                    status = "Hired";
                                    break;
                                case -1:
                                    status = "Rejected";
                                    break;
                                default:
                                    status = "Contact the Recuiter";
                                    break;


                            }
                            CandidateCheckOneJobViewModel viewModelFound = new CandidateCheckOneJobViewModel
                            {
                                Status = status,
                                Job = job
                            };
                            return View("DetailsForCandidate", viewModelFound);
                        }


                    }
                }
               
                // If no candidate applies this job
                CandidateCheckOneJobViewModel viewModel = new CandidateCheckOneJobViewModel
                {
                    Status = "No Apply",
                    Job = job
                };
                return View("DetailsForCandidate", viewModel);
            }
            else if(Int32.Parse(Session["userPrivilege"].ToString()) == 2)
            {
                return View("DetailsForHiringManager", job);
            }
            else
            {
                return View("DetailsForGuest", job);
            }
            
        }

        public ActionResult Edit(int id)
        {
            var jobInDb = _context.Jobs.Include(j => j.Degree).SingleOrDefault(j => j.Id == id);
            var degrees = _context.Degrees.ToList();
            var viewModel = new JobFormViewModel
            {
                Degrees = degrees,
                Job = jobInDb
                
            };
            return View("JobForm", viewModel);

        }

        //One candidate can only apply one job once, the checking process is in ResumeFilesController
        // candidateList form: "$candidateId_resumeId_statusId"
        public ActionResult Apply(int id, int fid)
        {
            var jobInDb = _context.Jobs.Single(j => j.Id == id);
            var list = jobInDb.CandidatesList;
            

            var newString = "$" + Session["id"].ToString() + "_" + fid.ToString() + "_1";
            list = list + newString;
            jobInDb.CandidatesList = list;

            int canId = Int32.Parse(Session["id"].ToString());
            var candidateInDb = _context.Candidates.Single(can => can.Id == canId);
            var jobList = candidateInDb.AppliedJobsList;
            var newJobString = "$" + id.ToString() + "_1";
            jobList = jobList + newJobString;
            candidateInDb.AppliedJobsList = jobList;

            _context.SaveChanges();
            return View("CandidateApplySuccessfully");
        }

        //only privilege == 2 user can access this method
        public ActionResult ShowCandidates(int id, string sortOrder)
        {
            if(Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
                
            }
            
            else
            {
                var list = _context.Jobs.Single(j => j.Id == id).CandidatesList;
                if (list == null)
                {
                    return View("NoCandidateApplied");
                }
                List<Candidate> candidates = new List<Candidate>();
                List<string> statuses = new List<string>();
                List<int> statusIds = new List<int>();
                List<int> resumeIds = new List<int>();
                string[] cs = list.Split('$');
                for (int i = 1; i < cs.Length; i++)
                {
                    string[] items = cs.ElementAt(i).Split('_');
                    var cid = Int32.Parse(items.ElementAt(0));
                    var rid = Int32.Parse(items.ElementAt(1));
                    resumeIds.Add(rid);
                    Candidate can = _context.Candidates.Single(cand => cand.Id == cid);
                    candidates.Add(can);
                    switch (Int32.Parse(items.ElementAt(2)))
                    {
                        case 0:
                            statuses.Add("No Apply");
                            statusIds.Add(0);
                            break;
                        case 1:
                            statuses.Add("Pending");
                            statusIds.Add(1);
                            break;
                        case 2:
                            statuses.Add("Reviewed");
                            statusIds.Add(2);
                            break;
                        case 3:
                            statuses.Add("Phone Interviewed");
                            statusIds.Add(3);
                            break;
                        case 4:
                            statuses.Add("Onsite Interviewed");
                            statusIds.Add(4);
                            break;
                        case 5:
                            statuses.Add("Offer Extended");
                            statusIds.Add(5);
                            break;
                        case 6:
                            statuses.Add("Hired");
                            statusIds.Add(6);
                            break;
                        case -1:
                            statuses.Add("Rejected");
                            statusIds.Add(-1);
                            break;
                        default:
                            statuses.Add("Contact the Recuiter");
                            statusIds.Add(0);
                            break;


                    }


                }
                
                // sorting by the status
                ViewBag.StatusSortParm = string.IsNullOrEmpty(sortOrder) ? "Status_desc" : "";
                
                switch (sortOrder)
                {
                    case "Status_desc":    //using bubble sorting method, stable sorting method
                        for(int i = 0; i < statusIds.Count() - 1; i++)
                        {
                            for(int j = 0; j < statusIds.Count() - 1; j++)
                            {
                                if(statusIds[j] < statusIds[j + 1])
                                {
                                    var tempId = statusIds[j];
                                    Candidate tempCandidate = candidates[j];
                                    string tempStatus = statuses[j];
                                    int tempResumeId = resumeIds[j];
                                    statusIds[j] = statusIds[j + 1];
                                    candidates[j] = candidates[j + 1];
                                    statuses[j] = statuses[j + 1];
                                    resumeIds[j] = resumeIds[j + 1];
                                    statusIds[j + 1] = tempId;
                                    candidates[j + 1] = tempCandidate;
                                    statuses[j + 1] = tempStatus;
                                    resumeIds[j + 1] = tempResumeId;

                                }
                            }
                        }
                        break;
                    

                    default:
                        for (int i = 0; i < statusIds.Count() - 1; i++)
                        {
                            for (int j = 0; j < statusIds.Count() - 1; j++)
                            {
                                if (statusIds[j] > statusIds[j + 1])
                                {
                                    var tempId = statusIds[j];
                                    Candidate tempCandidate = candidates[j];
                                    string tempStatus = statuses[j];
                                    statusIds[j] = statusIds[j + 1];
                                    candidates[j] = candidates[j + 1];
                                    statuses[j] = statuses[j + 1];
                                    statusIds[j + 1] = tempId;
                                    candidates[j + 1] = tempCandidate;
                                    statuses[j + 1] = tempStatus;

                                }
                            }
                        }
                        break;
                }
                
                JobCandidatesViewModel viewModel = new JobCandidatesViewModel
                {
                    Candidates = candidates,
                    Status = statuses,
                    ResumeIds = resumeIds
                    
                };
                return View(viewModel);
            }
        }


        // change the statusId in the candidateList of job and appliedJobsList of candidate
        public ActionResult ChangeStatus(int cid)
        {
            if (Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");

            }
            else
            {
                // change candidateList of the job
                var jid = Int32.Parse(Session["viewingJob"].ToString());
                Job jobInDb = _context.Jobs.Single(j => j.Id == jid);
                string[] cands = jobInDb.CandidatesList.Split('$');
                string newCandidatesList = "";
                for(int i = 1; i < cands.Length; i++)
                {
                    string[] items = cands.ElementAt(i).Split('_');
                    int candId = Int32.Parse(items.ElementAt(0));
                    if(candId == cid)
                    {
                        string item1 = items.ElementAt(0);
                        string item2 = items.ElementAt(1);
                        string item3 = (Int32.Parse(items.ElementAt(2)) + 1).ToString();
                        string newCand = "$" + item1 + "_" + item2 + "_" + item3;
                        newCandidatesList += newCand;
                    }
                    else
                    {
                        newCandidatesList += "$" + cands.ElementAt(i);
                    }
                }
                jobInDb.CandidatesList = newCandidatesList;

                // change the appliedJobsList of the candidate
                Candidate candidateInDb = _context.Candidates.Single(c => c.Id == cid);
                string[] jobs = candidateInDb.AppliedJobsList.Split('$');
                string newJobsList = "";
                for (int i = 1; i < jobs.Count(); i++)
                {
                    string[] items = jobs.ElementAt(i).Split('_');
                    int jobId = Int32.Parse(items.ElementAt(0));
                    if (jobId == jid)
                    {
                        string item1 = items.ElementAt(0);
                        
                        string item2 = (Int32.Parse(items.ElementAt(1)) + 1).ToString();
                        string newJob = "$" + item1 + "_"  + item2;
                        newJobsList += newJob;
                    }
                    else
                    {
                        newJobsList += "$" + jobs.ElementAt(i);
                    }
                }
                candidateInDb.AppliedJobsList = newJobsList;

                _context.SaveChanges();
                return RedirectToAction("ShowCandidates", new { id = jid});
            }
        }


        // change the statusId to -1, set as rejected
        public ActionResult RejectCandidate(int cid)
        {
            if (Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");

            }
            else
            {
                // change candidateList of the job
                var jid = Int32.Parse(Session["viewingJob"].ToString());
                Job jobInDb = _context.Jobs.Single(j => j.Id == jid);
                string[] cands = jobInDb.CandidatesList.Split('$');
                string newCandidatesList = "";
                for (int i = 1; i < cands.Length; i++)
                {
                    string[] items = cands.ElementAt(i).Split('_');
                    int candId = Int32.Parse(items.ElementAt(0));
                    if (candId == cid)
                    {
                        string item1 = items.ElementAt(0);
                        string item2 = items.ElementAt(1);
                        int val = -1;
                        string item3 = val.ToString();
                        string newCand = "$" + item1 + "_" + item2 + "_" + item3;
                        newCandidatesList += newCand;
                    }
                    else
                    {
                        newCandidatesList += "$" + cands.ElementAt(i);
                    }
                }
                jobInDb.CandidatesList = newCandidatesList;

                // change the appliedJobsList of the candidate
                Candidate candidateInDb = _context.Candidates.Single(c => c.Id == cid);
                string[] jobs = candidateInDb.AppliedJobsList.Split('$');
                string newJobsList = "";
                for (int i = 1; i < jobs.Count(); i++)
                {
                    string[] items = jobs.ElementAt(i).Split('_');
                    int jobId = Int32.Parse(items.ElementAt(0));
                    if (jobId == jid)
                    {
                        string item1 = items.ElementAt(0);
                        var val = -1;
                        string item2 = val.ToString();
                        string newJob = "$" + item1 + "_" + item2;
                        newJobsList += newJob;
                    }
                    else
                    {
                        newJobsList += "$" + jobs.ElementAt(i);
                    }
                }
                candidateInDb.AppliedJobsList = newJobsList;

                _context.SaveChanges();
                return RedirectToAction("ShowCandidates", new { id = jid });
            }
        }
    }
}