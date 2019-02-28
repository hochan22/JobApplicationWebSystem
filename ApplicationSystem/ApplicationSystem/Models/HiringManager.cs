using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationSystem.Models
{
    public class HiringManager
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public byte Privilege { get; set; }
        public string Code { get; set; }
    }
}