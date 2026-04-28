using EMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS_Project.Controllers
{
    
    public class AccountController : Controller
    {
        // GET: Account
        EMS_DBEntities db = new EMS_DBEntities();

        

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            var user = db.Users.FirstOrDefault(u => u.UserName == username && u.Password == password);

            if(user != null)
            {
                Session["User"] = user.UserName;
                Session["Role"] = user.Role;

                if(user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return RedirectToAction("Index", "Employee");
                }

            }

            ViewBag.Error = "Invalid username or password";
            return View();
        }
    }
}