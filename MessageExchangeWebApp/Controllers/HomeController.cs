using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageExchangeWebApp.Models;
using System.Data.Entity;

namespace MessageExchangeWebApp.Controllers
{
    public class HomeController : Controller
    {
        MessageExchangeContext db = new MessageExchangeContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult CreateMessage()
        {
            SelectList userlist = new SelectList(db.Users, "Login", "Login");
            ViewBag.List = userlist;

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateMessage(CreateMessageModel model)
        {
            model.Date = DateTime.Now.ToString();
            model.SrcUserLogin = User.Identity.Name;

            SelectList userlist = new SelectList(db.Users, "Login", "Login");
            ViewBag.List = userlist;

            if (ModelState.IsValid)
            {
                int _UserIdSrc = db.Users.FirstOrDefault(u => u.Login == model.SrcUserLogin).Id;

                User user = db.Users.FirstOrDefault(u => u.Login == model.DstUserLogin);
                if (user == null)
                {
                    ModelState.AddModelError("", "Такого пользователя не существует");
                    return View(model);
                }

                int _UserIdDst = user.Id;

                Chat c;
                c = db.Chats.Where(u => u.UserIdSrc == _UserIdSrc && u.UserIdDst == _UserIdDst).FirstOrDefault();
                if (c == null)
                {
                    c = new Chat { UserIdSrc = _UserIdSrc, UserIdDst = _UserIdDst };
                    db.Chats.Add(c);
                    db.SaveChanges();
                }

                Message m = new Message { Content = model.Content, Date = model.Date, Chat = c };
                db.Messages.Add(m);
                db.SaveChanges();

                ModelState.AddModelError("", "Сообщение отправлено!");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult ListChats()
        {
            var c = db.Chats.Include(u => u.UserSrc).Include(u => u.UserDst);
            return View(c.ToList());
        }

        [Authorize]
        public ActionResult DeleteChat(int id)
        {
            Chat c = new Chat { Id = id };
            db.Entry(c).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("ListChats");
        }

        [Authorize]
        public ActionResult ListMessagesOfChat(int id)
        {
            Chat c = db.Chats.Include(u => u.UserSrc).Include(u => u.UserDst).Include(u => u.Messages).FirstOrDefault(p => p.Id == id);
            return View(c);
        }

        [Authorize]
        public ActionResult DeleteMessage(int id)
        {
            Message b = new Message { Id = id };
            db.Entry(b).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("ListChats");
        }
    }
}