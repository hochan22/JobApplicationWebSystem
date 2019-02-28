using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ApplicationSystem.Models;

namespace ApplicationSystem.ViewModels
{
    public class MessageViewModel
    {
        public List<Message> SentMessages { get; set; }
        public List<Message> ReceivedMessages { get; set; }
        
    }
}