using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public class Instructor
    {
        public int ID { get; set; }
        [StringLength(50)]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        [StringLength(50,MinimumLength =1,ErrorMessage ="Maximum 50 characters and minimum 0 character")]
        [Required]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="{0:yyyy-MM-dd}")]
        [Display(Name ="Hire Date")]
        public DateTime HireDate { get; set; }
        public string FullName {
            get {
                return FirstName+" "+ LastName;
            }
        
        
        }
        public virtual ICollection<Course>Courses { get; set; }
        public virtual OfficeAssignment OfficeAssignment { get; set; }

    }
}