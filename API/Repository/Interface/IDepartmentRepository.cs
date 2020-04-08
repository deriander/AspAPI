using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Repository.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<DepartmentModel> Get();
        Task<IEnumerable<DepartmentModel>> Get(int Id);
        int Create(DepartmentModel department);
        int Update(int Id, DepartmentModel department);
        int Delete(int Id);
    }
}
