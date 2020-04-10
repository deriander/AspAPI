using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class DivisionViewModel {
        
        public int Id { get; set; }
        public string DivisionName { get; set; }
        public string DepartmentName { get; set; }
        public Nullable<DateTimeOffset> CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
        public int DeptID { get; set; }

    }
}