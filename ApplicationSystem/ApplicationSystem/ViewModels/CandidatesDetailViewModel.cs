using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSystem.Models;

namespace ApplicationSystem.ViewModels
{
    public class CandidatesDetailViewModel
    {
        public Candidate Candidate { get; set; }
        public List<string> Notes { get; set; }
    }
}