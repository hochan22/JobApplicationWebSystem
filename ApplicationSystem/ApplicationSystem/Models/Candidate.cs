using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationSystem.Models
{
    public class Candidate
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string University { get; set; }
        public string Major { get; set; }
        public Degree Degree { get; set; }
        public byte DegreeId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Privilege { get; set; }
        public string Notes { get; set; }
        public string AppliedJobsList { get; set; }
    }
}