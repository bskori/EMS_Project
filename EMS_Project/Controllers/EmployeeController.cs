using EMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS_Project.Controllers
{
    public class EmployeeController : Controller
    {
        EMS_DBEntities db = new EMS_DBEntities();

        public ActionResult Index()
        {
            var employees = db.Employees.Include("Department").ToList();
            return View(employees);
        }

        public ActionResult Create()
        {
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "DeptName");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Employee emp)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(emp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "DeptName");
            return View(emp);
        }
    }
}