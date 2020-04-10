using API.Models;
using API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    public class DepartmentController : ApiController
    {
        DepartmentRepository department = new DepartmentRepository();

        // get all
        [HttpGet]
        public IEnumerable<DepartmentModel> Get()
        {
            return department.Get();
        }

        //get by id
        [HttpGet]
        [ResponseType(typeof(DepartmentModel))]
        public async Task<IEnumerable<DepartmentModel>> Get(int Id)
        {
            return await department.Get(Id);
        }

        //create
        public IHttpActionResult Post(DepartmentModel departments)
        {

            if (departments.Name != null && departments.Name != "")
            {
                var post = department.Create(departments);
                if (post > 0)
                {
                    return Ok("Department Added Succesfully!");
                }
            }   
            return BadRequest("Failed to add Department");
   
        }

        //update
        public IHttpActionResult Put(int Id, DepartmentModel departments)
        {
            
            if (departments.Name != null && departments.Name != "")
            {
                var put = department.Update(Id, departments);
                if (put > 0)
                {
                    return Ok("Department Updated Succesfully!");
                }          
            }
            return BadRequest("Failed to updated Department");

        }

        //delete
        public IHttpActionResult Delete(int Id)
        {
            var delete = department.Delete(Id);
            if (delete > 0)
            {
                return Ok("Department Deleted Succesfully!");
            }
            return BadRequest("Failed to deleted Department");
        }

    }
}
