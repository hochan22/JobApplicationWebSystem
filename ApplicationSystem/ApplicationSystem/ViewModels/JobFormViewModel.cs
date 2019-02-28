using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSystem.Models;

namespace ApplicationSystem.ViewModels
{
    public class JobFormViewModel
    {
        public Job Job { get; set; }
        public IEnumerable<Degree> Degrees { get; set; }
        
    }
}