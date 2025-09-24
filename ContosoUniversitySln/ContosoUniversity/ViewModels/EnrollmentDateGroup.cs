using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContosoUniversity.ViewModels
{
    public class EnrollmentDateGroup
    {
        public DateTime ? EnrollmentDate { get; set; }
        public int StudentCount { get; set; }
    }
}