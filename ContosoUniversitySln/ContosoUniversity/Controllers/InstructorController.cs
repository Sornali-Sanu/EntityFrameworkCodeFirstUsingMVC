using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using ContosoUniversity.ViewModels;

namespace ContosoUniversity.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructor
        public ActionResult Index(int? id,int? courseId)
        {
            var viewModel = new InstructorIndexData();
            viewModel.Instructors = db.Instructors.Include(i => i.OfficeAssignment).Include(i => i.Courses.Select(c => c.Department)).OrderBy(i => i.LastName).ToList();
            if (id != null)
            {
                ViewBag.InstructorID = id.Value;
                viewModel.Courses = viewModel.Instructors.Where(i => i.ID == id.Value).Single().Courses;
            }
            if (courseId != null)
            {
                ViewBag.CourseID = courseId.Value;
                //viewModel.Enrollments=viewModel.Courses.Where(x=>x.CourseID==courseId).Single().Enrollments;
                var selectedCourse = viewModel.Courses.Where(x => x.CourseID == courseId).Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.Student).Load();
                }
                viewModel.Enrollments=selectedCourse.Enrollments;
                
            }
            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            var instructor=new Instructor();
            instructor.Courses= new List<Course>();
            populatedAssignedData(instructor);
            return View();

        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LastName,FirstName,HireDate,OfficeAssignment")] Instructor instructor,string[] selectedCourses)
        {
            if (selectedCourses != null)
            {
                instructor.Courses= new List<Course>();
                foreach (var item in selectedCourses)
                {
                    var courseToAdd = db.Courses.Find(int.Parse(item));
                    instructor.Courses.Add(courseToAdd);

                }
            }
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            populatedAssignedData(instructor);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Include(i => i.OfficeAssignment).Include(i=>i.Courses).Where(i => i.ID == id).Single();
            populatedAssignedData(instructor);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        private void populatedAssignedData(Instructor instructor)
        {
            var allCourses = db.Courses;
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));
            var viewModel = new List<AssignedCourseData>();
            foreach (var item in allCourses)
            {
                viewModel.Add(new AssignedCourseData { 
                CourseID= item.CourseID,
                Title=item.Title,
                Assigned=instructorCourses.Contains(item.CourseID)
                });

            }
            ViewBag.Courses= viewModel;
        }

        [HttpPost,ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id, string[] selectedCourses)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var instructorToUpdate = db.Instructors.Include(i => i.OfficeAssignment).Include(i=>i.Courses).Where(i => i.ID == id).Single();
            if (TryUpdateModel(instructorToUpdate, "", new string[] { "LastName", "FirstName", "HireDate", "OfficeAssignment" }))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(instructorToUpdate.OfficeAssignment.Location))
                    {
                        instructorToUpdate.OfficeAssignment = null;
                    }
                    UpdateInstructorCourses(selectedCourses, instructorToUpdate);
                    db.Entry(instructorToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {

                    ModelState.AddModelError("", "Unable to save changes,try again");
                }
               
            }
            populatedAssignedData(instructorToUpdate);
            return View(instructorToUpdate);

        }

        private void UpdateInstructorCourses(string[] selectedCourses, Instructor instructorToUpdate)
        {
            if (selectedCourses == null)
            {
                instructorToUpdate.Courses = new List<Course>();
                return;
            }
            var selectedCourseHS = new HashSet<string>(selectedCourses);
            var instructorCoursesHs=new HashSet<int>(instructorToUpdate.Courses.Select(c=>c.CourseID));
            foreach (var item in db.Courses)
            {
                if (selectedCourseHS.Contains(item.CourseID.ToString()))
                {
                    if (!instructorCoursesHs.Contains(item.CourseID))
                    {
                        instructorToUpdate.Courses.Add(item);
                    }
                }
                else{
                    if (instructorCoursesHs.Contains(item.CourseID))
                    {
                        instructorToUpdate.Courses.Remove(item);
                    }
                }

            }
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Include(i=>i.OfficeAssignment).Where(i=>i.ID==id).Single();
            instructor.OfficeAssignment=null;

            db.Instructors.Remove(instructor);
            var department = db.Departments.Where(i => i.InstructorID == id).SingleOrDefault();
            if (department != null)
            {
                department.InstructorID = null;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
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
