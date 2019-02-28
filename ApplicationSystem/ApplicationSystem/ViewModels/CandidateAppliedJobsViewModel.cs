using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationSystem.ViewModels
{
    public class CandidateAppliedJobsViewModel
    {
        public List<string> AppliedJobs { get; set; }
        public List<string> Statuses { get; set; }
        public List<int> AppliedJobsId { get; set; }
    }
}