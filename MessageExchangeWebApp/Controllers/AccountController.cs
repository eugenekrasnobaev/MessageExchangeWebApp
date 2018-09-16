using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MessageExchangeWebApp.Models;
using System.Web.Security;
using System.Data.Entity;

namespace MessageExchangeWebApp.Controllers
{
    public class AccountController : Controller
    {
        MessageExchangeContext db = new MessageExchangeContext();

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("ListChats", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Пользователя с таким логином и паролем нет");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            model.Role = "user";
            if (ModelState.IsValid)
            {
                User user = null;
                user = db.Users.FirstOrDefault(u => u.Login == model.Login);

                if (user == null)
                {
                    db.Users.Add(new User { Name = model.Name, Surname = model.Surname, Login = model.Login, Password = model.Password, E_mail = model.E_mail, Role = model.Role });
                    db.SaveChanges();
                    user = db.Users.Where(u => u.Login == model.Login && u.Password == model.Password).FirstOrDefault();
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("ListChats", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Пользователь с таким логином уже существует");
            }
            return View(model);
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "admin")]
        public ActionResult ListUsers()
        {
            return View(db.Users);       
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(int id)
        {
            User u = new User { Id = id };
            db.Entry(u).State = EntityState.Deleted;
            db.SaveChanges();

            return RedirectToAction("ListUsers");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            User u = db.Users.Find(id);
            if (u != null)
            {
                return View(u);
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditUser(User u)
        {
            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("ListUsers");
        }
    }
}