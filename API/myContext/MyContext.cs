using API.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace API.myContext
{
    public class MyContext : DbContext
    {
        public MyContext() : base("AspAPI") { }
        public DbSet<DepartmentModel> Departments { get; set; }
        public DbSet<DivisionModel> Divisions { get; set; }
    }
}