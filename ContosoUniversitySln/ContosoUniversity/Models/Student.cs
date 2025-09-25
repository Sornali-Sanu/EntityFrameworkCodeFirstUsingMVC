using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls.WebParts;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50,ErrorMessage = "Last Name cannot be longer than 50 characters  ")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$")]
        [Display(Name="Last Name")]
        public string LastName { get; set; }
        [StringLength(50, MinimumLength = 1, ErrorMessage ="First Name cannot be longer than 50 characters and must be Filled!!! ")]
        [Column("First Name")]
        [Required]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}",ApplyFormatInEditMode =true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }
        public string FullName 
        { 
            get 
            { return FirstName + " " + LastName; } }
        public virtual ICollection<Enrollment> Enrollments { get; set; }
      

    }
}