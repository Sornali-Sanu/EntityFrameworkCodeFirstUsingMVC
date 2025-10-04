using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ContosoUniversity.Models
{
    public abstract class Person
    {
        public int ID { get; set; }
        [StringLength(50,ErrorMessage ="Last Name can not be higher 50 character")]
        [Display(Name ="Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(50, ErrorMessage = "First Name can not be higher 50 character")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name ="Full Name" )]
        public string FullName { get { return LastName + " " + FirstName; } }
    }
}