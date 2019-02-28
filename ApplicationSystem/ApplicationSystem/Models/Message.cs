using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApplicationSystem.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string Sender { get; set; }
        public int SenderPrivilege { get; set; }
        public int ReceiverId { get; set; }
        public string Receiver { get; set; }
        public int ReceiverPrivilege { get; set; }
        public string Content { get; set; }
    }
}