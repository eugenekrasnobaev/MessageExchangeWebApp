using System;
using System.Linq;
using System.Web.Mvc;
using MessageExchangeWebApp.Models;
using System.Data.Entity;

namespace MessageExchangeWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MessageExchangeContext _db = new MessageExchangeContext();

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
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
            var userlist = new SelectList(_db.Users, "Login", "Login");
            ViewBag.List = userlist;

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateMessage(CreateMessageModel model)
        {
            model.Date = DateTime.Now;
            model.SrcUserLogin = User.Identity.Name;

            var userlist = new SelectList(_db.Users, "Login", "Login");
            ViewBag.List = userlist;

            int userIdSrc, userIdDst;

            if (ModelState.IsValid)
            {
                // определение id пользователя отправителя сообщения
                var user = _db.Users
                    .FirstOrDefault(u => u.Login == model.SrcUserLogin);
                if (user == null)
                {
                    return HttpNotFound();
                }
                userIdSrc = user.Id;

                // определение id пользователя получателя сообщения
                user = _db.Users
                    .FirstOrDefault(u => u.Login == model.DstUserLogin);
                if (user == null)
                {
                    return HttpNotFound();
                }
                userIdDst = user.Id;

                // поиск чата с заданным отправителем и получателем
                var c = _db.Chats
                    .FirstOrDefault(u => u.UserIdSrc == userIdSrc && u.UserIdDst == userIdDst);

                // если такого чата нет - создаем его
                if (c == null)
                {
                    c = new Chat { UserIdSrc = userIdSrc, UserIdDst = userIdDst };
                    _db.Chats.Add(c);
                    _db.SaveChanges();
                }

                // создаем сообщение в своем чате
                var m = new Message { Content = model.Content, Date = model.Date, Chat = c };
                _db.Messages.Add(m);
                _db.SaveChanges();

                ModelState.AddModelError("", "Сообщение отправлено!");
            }

            return View(model);
        }

        [Authorize]
        public ActionResult ListChats()
        {
            var c = _db.Chats
                .Include(u => u.UserSrc)
                .Include(u => u.UserDst);
            return View(c.ToList());
        }

        [Authorize]
        public ActionResult DeleteChat(int id)
        {
            var c = new Chat { Id = id };
            _db.Entry(c).State = EntityState.Deleted;
            _db.SaveChanges();

            return RedirectToAction("ListChats", "Home");
        }

        [Authorize]
        public ActionResult ListMessagesOfChat(int id)
        {
            var c = _db.Chats
                .Include(u => u.UserSrc)
                .Include(u => u.UserDst)
                .Include(u => u.Messages)
                .FirstOrDefault(p => p.Id == id);
            return View(c);
        }

        [Authorize]
        public ActionResult DeleteMessage(int id)
        {
            var m = new Message { Id = id };
            _db.Entry(m).State = EntityState.Deleted;
            _db.SaveChanges();

            return RedirectToAction("ListChats", "Home");
        }
    }
}