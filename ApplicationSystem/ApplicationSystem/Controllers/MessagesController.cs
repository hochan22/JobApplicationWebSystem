using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ApplicationSystem.Models;
using ApplicationSystem.ViewModels;

namespace ApplicationSystem.Controllers
{
    public class MessagesController : Controller
    {
        ApplicationDbContext _context;

        public MessagesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // show message table of the user
        // GET: Messages
        public ActionResult Index()
        {
            if(Session["userPrivilege"] == null || Int32.Parse(Session["userPrivilege"].ToString()) != 1 && Int32.Parse(Session["userPrivilege"].ToString()) != 2)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            var messages = _context.Messages.ToList();
            var sentMessages = new List<Message>();
            var receivedMessages = new List<Message>();
            foreach(var message in messages)
            {
                if(message.Sender == Session["userName"].ToString())
                {
                    sentMessages.Add(message);
                }
                else if(message.Receiver == Session["userName"].ToString())
                {
                    receivedMessages.Add(message);
                }
            }
            receivedMessages.Reverse();
            sentMessages.Reverse();
            var messageViewModel = new MessageViewModel
            {
                SentMessages = sentMessages,
                ReceivedMessages = receivedMessages
                
            };

            return View(messageViewModel);
        }

        public ActionResult New()
        {
            if (Session["userPrivilege"] == null)
            {
                return RedirectToAction("SignIn", "UserAccount");
            }
            return View("MessageForm");
        }

        [HttpPost]
        public ActionResult Save(Message message)
        {
            message.Sender = Session["userName"].ToString();
            message.SenderId = Int32.Parse(Session["id"].ToString());
            message.SenderPrivilege = Int32.Parse(Session["userPrivilege"].ToString());
            var AccountInCandidates = _context.Candidates.ToList();
            // check the receiver in two tables
            foreach(var account in AccountInCandidates)
            {
                if(account.Name.Equals(message.Receiver) == true)
                {
                    message.ReceiverPrivilege = 1;
                    message.Receiver = account.Name;
                    message.ReceiverId = account.Id;
                    _context.Messages.Add(message);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Messages");
                }
            }
            var AccountInManagers = _context.HiringManagers.ToList();
            foreach (var account in AccountInManagers)
            {
                if (account.Name.Equals(message.Receiver) == true)
                {
                    message.ReceiverPrivilege = 2;
                    message.Receiver = account.Name;
                    message.ReceiverId = account.Id;
                    _context.Messages.Add(message);
                    _context.SaveChanges();
                    return RedirectToAction("Index", "Messages");
                }
            }
            // if the reveiver is invalid, show error page
            return View("InvalidReceiver");
        }
    }
}