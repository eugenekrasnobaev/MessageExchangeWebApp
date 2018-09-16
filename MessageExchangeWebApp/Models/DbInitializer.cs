using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace MessageExchangeWebApp.Models
{
    public class DbInitializer : DropCreateDatabaseAlways<MessageExchangeContext>
    {
        protected override void Seed(MessageExchangeContext db)
        {
            db.Users.Add(new User { Name = "Иванов", Surname = "Иван", Login = "Ivan", Password = "12345", E_mail = "ivan_ivanov@mail.ru", Role = "admin" });
            db.Users.Add(new User { Name = "Сидоров", Surname = "Сидор", Login = "Sidor", Password = "12345", E_mail = "sidor_sidorov@mail.ru", Role = "user" });
            db.Users.Add(new User { Name = "Петров", Surname = "Петр", Login = "Petr", Password = "12345", E_mail = "petr_petrov@mail.ru", Role = "user" });

            base.Seed(db);
        }
    }
}