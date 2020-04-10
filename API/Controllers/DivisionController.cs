using API.Models;
using API.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace API.Controllers
{
    public class DivisionController : ApiController
    {
        DivisionRepository division = new DivisionRepository();

        // get all
        [HttpGet]
        public IEnumerable<DivisionViewModel> Get()
        {
            return division.Get();
        }

        //get by id
        [HttpGet]
        [ResponseType(typeof(DivisionViewModel))]
        public async Task<IEnumerable<DivisionViewModel>> Get(int Id)
        {
            return await division.Get(Id);
        }

        //create
        public IHttpActionResult Post(DivisionModel divisions)
        {

            if (divisions.Name != null && divisions.Name != "")
            {
                var post = division.Create(divisions);
                if (post > 0)
                {
                    return Ok("Division Added Succesfully!");
                }
            }
            return BadRequest("Failed to add Division");

        }

        //update
        public IHttpActionResult Put(int Id, DivisionModel divisions)
        {

            if (divisions.Name != null && divisions.Name != "")
            {
                var put = division.Update(Id, divisions);
                if (put > 0)
                {
                    return Ok("Division Updated Succesfully!");
                }
            }
            return BadRequest("Failed to updated Division");

        }

        //delete
        public IHttpActionResult Delete(int Id)
        {
            var delete = division.Delete(Id);
            if (delete > 0)
            {
                return Ok("Division Deleted Succesfully!");
            }
            return BadRequest("Failed to deleted Division");
        }
    }
}