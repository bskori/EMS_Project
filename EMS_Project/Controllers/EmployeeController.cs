using EMS_Project.Models;
using PagedList;
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

        public ActionResult Index(string search, int? deptId, int? page, string sortOrder)
        {
            var employees = db.Employees.Include("Department").AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(x => x.Name.Contains(search));
            }
            if(deptId != null)
            {
                employees = employees.Where(x => x.DeptId == deptId);
            }

            ViewBag.Departments = new SelectList(db.Departments, "DeptId", "DeptName");
            
            int pageSize = 5;
            int pageNumber = (page ?? 1);

            ViewBag.SortName = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Name);
                    break;

                default:
                    employees = employees.OrderBy(e => e.Name);
                    break;
            }

            return View(employees.ToPagedList(pageNumber,pageSize));
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

        public ActionResult Edit(int id)
        {
            var emp = db.Employees.Find(id);
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "DeptName", emp.DeptId);
            return View(emp);
        }

        [HttpPost]
        public ActionResult Edit(Employee emp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(emp).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeptId = new SelectList(db.Departments, "DeptId", "DeptName", emp.DeptId);
            return View(emp);
        }

        public ActionResult Delete(int id)
        {
            var emp = db.Employees.Find(id);
            return View(emp);
        }

        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var emp = db.Employees.Find(id);
            db.Employees.Remove(emp);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}