using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSystem.Models;

namespace ApplicationSystem.ViewModels
{
    public class CandidateFormViewModel
    {
        public Candidate Candidate { get; set; }
        public IEnumerable<Degree> Degrees { get; set; }
        
    }
}