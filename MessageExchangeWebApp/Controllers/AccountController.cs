using System.Linq;
using System.Web.Mvc;
using MessageExchangeWebApp.Models;
using System.Web.Security;
using System.Data.Entity;

namespace MessageExchangeWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly MessageExchangeContext _db = new MessageExchangeContext();

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
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
                var user = _db.Users
                    .FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("ListChats", "Home");
                }

                ModelState.AddModelError("", "Неправильный логин или пароль!");
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
                var user = _db.Users
                    .FirstOrDefault(u => u.Login == model.Login);
                
                if (user == null)
                {
                    _db.Users.Add(new User { Name = model.Name, Surname = model.Surname, Login = model.Login, Password = model.Password, Email = model.Email, Role = model.Role });
                    _db.SaveChanges();

                    user = _db.Users
                        .FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);
                }

                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Name, true);
                    return RedirectToAction("ListChats", "Home");
                }

                ModelState.AddModelError("", "Логин занят!");
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
            return View(_db.Users);       
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteUser(int id)
        {
            var u = new User { Id = id };

            _db.Entry(u).State = EntityState.Deleted;
            _db.SaveChanges();

            return RedirectToAction("ListUsers", "Account");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult EditUser(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var u = _db.Users.Find(id);

            if (u != null)
            {
                return View(u);
            }
            return HttpNotFound();
        }

        [Authorize(Roles = "admin")]
        public ActionResult EditUser(User u)
        {
            _db.Entry(u).State = EntityState.Modified;
            _db.SaveChanges();
            return RedirectToAction("ListUsers", "Account");
        }
    }
}