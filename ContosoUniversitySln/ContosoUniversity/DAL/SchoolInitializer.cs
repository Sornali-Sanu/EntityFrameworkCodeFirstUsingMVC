using ContosoUniversity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.DAL
{
    public class SchoolInitializer:System.Data.Entity.DropCreateDatabaseIfModelChanges<SchoolContext>

    {
        protected override void Seed(SchoolContext context)
        {
            var students = new List<Student> {
            new Student{ FirstName="Sornali",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-12-12")},
            new Student{ FirstName="Tagin",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-10-12")},
            new Student{ FirstName="Seasen",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-11-12")},
            new Student{ FirstName="Jimmy",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-12-5")},
            new Student{ FirstName="Tasmin",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-12-11")},
            new Student{ FirstName="Nusrat",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-09-12")},
            new Student{ FirstName="Nishat",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-07-12")},
            new Student{ FirstName="Jisa",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-02-12")},
            new Student{ FirstName="Latif",LastName="Akter",EnrollmentDate=DateTime.Parse("2018-05-12")},
            new Student{ FirstName="Safrina",LastName="Akter",EnrollmentDate=DateTime.Parse("2017-12-12")},
          
            };
            students.ForEach(s => context.Students.Add(s));
            context.SaveChanges(); 

            var courses = new List<Course>
            {
                new Course{CourseID=1050,Title="Chemistry",Credits=4 },
                 new Course{CourseID=1051,Title="Physics",Credits=3 },
                  new Course{CourseID=1052,Title="Biology",Credits=2 },
                   new Course{CourseID=1053,Title="Math",Credits=4 },
                    new Course{CourseID=1054,Title="Calculas",Credits=3 },
                     new Course{CourseID=1055,Title="Literature",Credits=3 },
                      new Course{CourseID=1056,Title="English",Credits=3 },
                      new Course{CourseID=1057,Title="SocialScience",Credits=2 },
                      new Course{CourseID=1058,Title="Home Economics",Credits=4 },
                      new Course{CourseID=1059,Title="Quantum Mechanics",Credits=3 },

            };
            courses.ForEach(c => context.Courses.Add(c));
            context.SaveChanges();

            var enrollments = new List<Enrollment>
            {
                new Enrollment{CourseID=1050,StudentID=1,Grade=Grade.A },
                 new Enrollment{CourseID=1051,StudentID=2,Grade=Grade.B },
                  new Enrollment{CourseID=1052,StudentID=3,Grade=Grade.A },
                   new Enrollment{CourseID=1053,StudentID=4,Grade=Grade.C  },
                    new Enrollment{CourseID=1054,StudentID = 5, Grade = Grade.F},
                     new Enrollment{CourseID=1055,StudentID = 6 },
                      new Enrollment{CourseID=1056,StudentID = 7
                      },
                      new Enrollment{CourseID=1057,StudentID = 8, Grade = Grade.A },
                      new Enrollment{CourseID=1058,StudentID = 9, Grade = Grade.D },
                      new Enrollment{CourseID=1059,StudentID = 10, Grade = Grade.B},

            };
            enrollments.ForEach(e => context.Enrollments.Add(e));
            context.SaveChanges();

        }
    }
}