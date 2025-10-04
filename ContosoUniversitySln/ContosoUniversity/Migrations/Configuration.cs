namespace ContosoUniversity.Migrations
{
    using ContosoUniversity.DAL;
    using ContosoUniversity.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ContosoUniversity.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ContosoUniversity.DAL.SchoolContext context)
        {
            // ------------------- STUDENTS -------------------
            var students = new List<Student>
    {
        new Student { FirstName = "Carson",   LastName = "Alexander", EnrollmentDate = DateTime.Parse("2010-09-01") },
        new Student { FirstName = "Meredith", LastName = "Alonso",    EnrollmentDate = DateTime.Parse("2012-09-01") },
        new Student { FirstName = "Arturo",   LastName = "Anand",     EnrollmentDate = DateTime.Parse("2013-09-01") },
        new Student { FirstName = "Gytis",    LastName = "Barzdukas",EnrollmentDate = DateTime.Parse("2012-09-01") },
        new Student { FirstName = "Yan",      LastName = "Li",        EnrollmentDate = DateTime.Parse("2012-09-01") },
        new Student { FirstName = "Peggy",    LastName = "Justice",   EnrollmentDate = DateTime.Parse("2011-09-01") },
        new Student { FirstName = "Laura",    LastName = "Norman",    EnrollmentDate = DateTime.Parse("2013-09-01") },
        new Student { FirstName = "Nino",     LastName = "Olivetto",  EnrollmentDate = DateTime.Parse("2005-09-01") }
    };

            students.ForEach(s => context.Students.AddOrUpdate(p => p.LastName, s));
            context.SaveChanges();

            // ------------------- INSTRUCTORS -------------------
            var instructors = new List<Instructor>
    {
        new Instructor { FirstName = "Kim",     LastName = "Abercrombie", HireDate = DateTime.Parse("1995-03-11") },
        new Instructor { FirstName = "Fadi",    LastName = "Fakhouri",    HireDate = DateTime.Parse("2002-07-06") },
        new Instructor { FirstName = "Roger",   LastName = "Harui",       HireDate = DateTime.Parse("1998-07-01") },
        new Instructor { FirstName = "Candace", LastName = "Kapoor",      HireDate = DateTime.Parse("2001-01-15") },
        new Instructor { FirstName = "Roger",   LastName = "Zheng",       HireDate = DateTime.Parse("2004-02-12") }
    };

            instructors.ForEach(i => context.Instructors.AddOrUpdate(p => p.LastName, i));
            context.SaveChanges();

            // ------------------- DEPARTMENTS -------------------
            var departments = new List<Department>
    {
        new Department { Name = "English",     Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorID = instructors.Single(i => i.LastName == "Abercrombie").ID },
        new Department { Name = "Mathematics", Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID = instructors.Single(i => i.LastName == "Fakhouri").ID },
        new Department { Name = "Engineering", Budget = 350000, StartDate = DateTime.Parse("2007-09-01"), InstructorID = instructors.Single(i => i.LastName == "Harui").ID },
        new Department { Name = "Economics",   Budget = 100000, StartDate = DateTime.Parse("2007-09-01"), InstructorID = instructors.Single(i => i.LastName == "Kapoor").ID }
    };

            departments.ForEach(d => context.Departments.AddOrUpdate(p => p.Name, d));
            context.SaveChanges();

            // ------------------- COURSES -------------------
            var courses = new List<Course>
    {
        new Course { CourseID = 1050, Title = "Chemistry",      Credits = 3, DepartmentID = departments.Single(d => d.Name == "Engineering").DepartmentID },
        new Course { CourseID = 4022, Title = "Microeconomics", Credits = 3, DepartmentID = departments.Single(d => d.Name == "Economics").DepartmentID },
        new Course { CourseID = 4041, Title = "Macroeconomics", Credits = 3, DepartmentID = departments.Single(d => d.Name == "Economics").DepartmentID },
        new Course { CourseID = 1045, Title = "Calculus",       Credits = 4, DepartmentID = departments.Single(d => d.Name == "Mathematics").DepartmentID },
        new Course { CourseID = 3141, Title = "Trigonometry",   Credits = 4, DepartmentID = departments.Single(d => d.Name == "Mathematics").DepartmentID },
        new Course { CourseID = 2021, Title = "Composition",    Credits = 3, DepartmentID = departments.Single(d => d.Name == "English").DepartmentID },
        new Course { CourseID = 2042, Title = "Literature",     Credits = 4, DepartmentID = departments.Single(d => d.Name == "English").DepartmentID }
    };

            courses.ForEach(c => context.Courses.AddOrUpdate(p => p.CourseID, c));
            context.SaveChanges();

            // ------------------- OFFICE ASSIGNMENTS -------------------
            var officeAssignments = new List<OfficeAssignment>
    {
        new OfficeAssignment { InstructorID = instructors.Single(i => i.LastName == "Fakhouri").ID, Location = "Smith 17" },
        new OfficeAssignment { InstructorID = instructors.Single(i => i.LastName == "Harui").ID,    Location = "Gowan 27" },
        new OfficeAssignment { InstructorID = instructors.Single(i => i.LastName == "Kapoor").ID,   Location = "Thompson 304" }
    };

            officeAssignments.ForEach(o => context.OfficeAssignments.AddOrUpdate(p => p.InstructorID, o));
            context.SaveChanges();

            // ------------------- COURSE INSTRUCTORS (many-to-many) -------------------
            AddOrUpdateInstructor(context, "Chemistry", "Kapoor");
            AddOrUpdateInstructor(context, "Chemistry", "Harui");
            AddOrUpdateInstructor(context, "Microeconomics", "Zheng");
            AddOrUpdateInstructor(context, "Macroeconomics", "Zheng");
            AddOrUpdateInstructor(context, "Calculus", "Fakhouri");
            AddOrUpdateInstructor(context, "Trigonometry", "Harui");
            AddOrUpdateInstructor(context, "Composition", "Abercrombie");
            AddOrUpdateInstructor(context, "Literature", "Abercrombie");
            context.SaveChanges();

            // ------------------- ENROLLMENTS -------------------
            var enrollments = new List<Enrollment>
    {
        new Enrollment { StudentID = students.Single(s => s.LastName == "Alexander").ID, CourseID = courses.Single(c => c.Title == "Chemistry").CourseID, Grade = Grade.A },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Alexander").ID, CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID, Grade = Grade.C },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Alexander").ID, CourseID = courses.Single(c => c.Title == "Macroeconomics").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Alonso").ID,    CourseID = courses.Single(c => c.Title == "Calculus").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Alonso").ID,    CourseID = courses.Single(c => c.Title == "Trigonometry").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Alonso").ID,    CourseID = courses.Single(c => c.Title == "Composition").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Anand").ID,     CourseID = courses.Single(c => c.Title == "Chemistry").CourseID },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Anand").ID,     CourseID = courses.Single(c => c.Title == "Microeconomics").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Barzdukas").ID, CourseID = courses.Single(c => c.Title == "Chemistry").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Li").ID,        CourseID = courses.Single(c => c.Title == "Composition").CourseID, Grade = Grade.B },
        new Enrollment { StudentID = students.Single(s => s.LastName == "Justice").ID,   CourseID = courses.Single(c => c.Title == "Literature").CourseID, Grade = Grade.B }
    };

            foreach (var e in enrollments)
            {
                var enrollmentInDB = context.Enrollments
                    .SingleOrDefault(s => s.StudentID == e.StudentID && s.CourseID == e.CourseID);

                if (enrollmentInDB == null)
                    context.Enrollments.Add(e);
            }

            context.SaveChanges();
        }

        // ------------------- HELPER METHOD -------------------
        private void AddOrUpdateInstructor(SchoolContext context, string courseTitle, string instructorLastName)
        {
            var crs = context.Courses.SingleOrDefault(c => c.Title == courseTitle);
            var inst = context.Instructors.SingleOrDefault(i => i.LastName == instructorLastName);

            if (crs == null)
                throw new Exception($"Course not found: {courseTitle}");
            if (inst == null)
                throw new Exception($"Instructor not found: {instructorLastName}");

            if (!crs.Instructors.Contains(inst))
                crs.Instructors.Add(inst);
        }
    }
}


