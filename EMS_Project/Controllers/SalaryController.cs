using EMS_Project.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS_Project.Controllers
{
    public class SalaryController : Controller
    {
        EMS_DBEntities db = new EMS_DBEntities();
        public ActionResult Create(int? empId)
        {
            var exists = db.Salaries.Any(s => s.EmpId == empId);

            if (exists)
            {
                return RedirectToAction("Index", "Employee");
            }

            var employeesWithoutSalary = db.Employees.Where(e => !db.Salaries.Any(s => s.EmpId == e.EmpId)).ToList();

            //ViewBag.EmpId = new SelectList(employeesWithoutSalary, "EmpId", "Name", empId);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Salary sal)
        {
            if (ModelState.IsValid)
            {
                var exists = db.Salaries.Any(s => s.EmpId == sal.EmpId);

                if (exists)
                {
                    ModelState.AddModelError("", "Salary already exists for this employee");
                }
                else
                {
                    sal.HRA = sal.BasicSalary * 0.10m;
                    sal.DA = sal.BasicSalary * 0.20m;
                    sal.PF = sal.BasicSalary * 0.10m;

                    sal.GrossSalary = sal.BasicSalary + sal.DA + sal.HRA - sal.PF;

                    db.Salaries.Add(sal);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Employee");
                }
                
            }
            var employeesWithoutSalary = db.Employees.Where(e => !db.Salaries.Any(s => s.EmpId == e.EmpId)).ToList();

            //ViewBag.EmpId = new SelectList(employeesWithoutSalary, "EmpId", "Name", sal.EmpId);
            return View(sal);
        }

        public ActionResult Details(int empId)
        {
            var salary = db.Salaries.FirstOrDefault(s => s.EmpId == empId);

            if(salary == null)
            {
                return HttpNotFound();
            }

            return View(salary);
        }
    }
}