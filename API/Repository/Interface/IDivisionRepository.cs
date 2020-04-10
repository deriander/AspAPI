using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Repository.Interface
{
    public interface IDivisionRepository
    {
        IEnumerable<DivisionViewModel> Get();
        Task<IEnumerable<DivisionViewModel>> Get(int Id);
        int Create(DivisionModel department);
        int Update(int Id, DivisionModel department);
        int Delete(int Id);
    }
}