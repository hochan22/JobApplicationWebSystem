using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ApplicationSystem.Models
{
    public class Job
    {
        public int Id { get; set; }

        [Display(Name = "Position Title")]
        public string Name { get; set; }

        [Display(Name = "Job Discription")]
        public string Discription { get; set; }


        public Degree Degree { get; set; }

        [Display(Name = "Degree Requirement")]
        public byte DegreeId { get; set; }


        public string Location { get; set; }

        [Display(Name = "Duration in Months")]
        public int DurationInMonths { get; set; }

        public string CandidatesList { get; set; } //"$1_1$2_0$3_2" 
       
    }
}