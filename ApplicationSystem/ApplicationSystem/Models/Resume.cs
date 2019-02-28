using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ApplicationSystem.Models
{
    public class Resume
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Select File")]
        public HttpPostedFileBase Files { get; set; }

        
    }
}