using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;
using PagedList;

namespace ContosoUniversity.Controllers
{
    public class StudentsController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Students
        public ActionResult Index(string sortOrder,string searchStrings,string currentFilter,int? page)
        {   ViewBag.currentSort=sortOrder; 
            ViewBag.NameSortParam = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSort = sortOrder == "Date" ? "date_desc" : "Date";
            ViewBag.lastNameParam = string.IsNullOrEmpty(sortOrder) ? "Lname_desc" : "";
            if (searchStrings != null)
            {
                page = 1;
            }
            else {
                searchStrings = currentFilter;
            }
            ViewBag.CurrentFilter = searchStrings;
                var students = from s in db.Students select s;
            if (!String.IsNullOrEmpty(searchStrings))
            {
                students = students.Where(s => s.FirstName.ToUpper().Contains(searchStrings.ToUpper())
                || s.LastName.ToUpper().Contains(searchStrings.ToUpper()));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(s => s.FirstName);
                        break;
                case "date_desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default :
                    students =students.OrderByDescending(s => s.LastName);
                    break;
            }
            int pageSize = 3;
            int pageNumber = (page ?? 1);

            return View(students.ToPagedList(pageNumber,pageSize));
        }

        // GET: Students/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Create
        public ActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to Save Changes.Try again");
              
            }
           

            return View(student);
        }

        // GET: Students/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,EnrollmentDate")] Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(student).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save Changes.Try again,if the Problem persists see the system administrator");

               
            }
          
            return View(student);
        }

        // GET: Students/Delete/5
        public ActionResult Delete(int? id,bool? saveChangesError=false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault()) {
                ViewBag.ErroMessage = "Delete failed .Try again,and if problem Presists see your administrator";
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Student student = db.Students.Find(id);
                db.Students.Remove(student);
                db.SaveChanges();

            }
            catch (DataException)
            {

                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
           
            return RedirectToAction("Index");
        }
        public ActionResult about()
        {
            IQueryable<EnrollmentDateGroup> data = from student in db.Students
                                                   group student by student.EnrollmentDate into dateGroup
                                                   select new EnrollmentDateGroup()
                                                   {
                                                       EnrollmentDate = dateGroup.Key,
                                                       StudentCount = dateGroup.Count()
                                                   };
            return View(data.ToList());
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
