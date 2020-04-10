using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace API.Models
{
    [Table("TB_M_Division")]
    public class DivisionModel
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsDelete { get; set; }
        public DateTimeOffset CreateDate { get; set; }
        public Nullable<DateTimeOffset> UpdateDate { get; set; }
        public Nullable<DateTimeOffset> DeleteDate { get; set; }
        public int DepartmentID { get; set; }
        public virtual DepartmentModel Department { get; set; }

        public DivisionModel()
        {

        }

        public DivisionModel(DivisionModel division)
        {
            this.Name = division.Name;
            this.CreateDate = DateTimeOffset.Now;
            this.IsDelete = false;
            this.DepartmentID = division.DepartmentID;
        }

        public void Update(DivisionModel division)
        {
            this.Name = division.Name;
            this.UpdateDate = DateTimeOffset.Now;
            this.DepartmentID = division.DepartmentID;
        }

        public void Delete()
        {
            this.IsDelete = true;
            this.DeleteDate = DateTimeOffset.Now;
        }
    }
}