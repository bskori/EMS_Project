using EMS_Project.Models;
using EMS_Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EMS_Project.Controllers
{
    public class DashboardController : Controller
    {
        EMS_DBEntities db = new EMS_DBEntities();
        public ActionResult Index()
        {
            ViewBag.TotalEmployees = db.Employees.Count();
            ViewBag.TotalDepartments = db.Departments.Count();
            ViewBag.HighestSalary = db.Salaries.Max(s => s.GrossSalary);

            var deptData = db.Departments.Select(d => new
            DeptEmployeeCountVM
            {
                DeptName = d.DeptName,
                Count = d.Employees.Count()
            }).ToList();


            return View(deptData);

        }
    }
}