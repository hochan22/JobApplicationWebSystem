using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSystem.Models;

namespace ApplicationSystem.ViewModels
{
    public class JobCandidatesViewModel
    {
        public List<Candidate> Candidates { get; set; }
        public List<string> Status { get; set; }
        public List<int> ResumeIds { get; set; }
    }
}