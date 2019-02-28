using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationSystem.Models
{
    public class PdfFile
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
    }
}