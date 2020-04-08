using API.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using API.Models;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using Dapper;
using System.Data;

namespace API.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        DynamicParameters parameters = new DynamicParameters();

        public int Create(DepartmentModel department)
        {
            var procedureName = "SP_InsertDepartment";
            parameters.Add("@Name", department.Name);
            var create = connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            var procedureName = "SP_DeleteDepartment";
            parameters.Add("@Id", Id);
            var deleteDept = connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return Convert.ToInt32(deleteDept);
        }

        public IEnumerable<DepartmentModel> Get()
        {
            var procedureName = "SP_ViewDepartment";
            var getAllDept = connection.Query<DepartmentModel>(procedureName, commandType: CommandType.StoredProcedure);
            return getAllDept;
        }

        public async Task<IEnumerable<DepartmentModel>> Get(int Id)
        {
            var procedureName = "SP_GetById";
            parameters.Add("@Id", Id);
            var getById = await connection.QueryAsync<DepartmentModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return getById;
        }

        public int Update(int Id, DepartmentModel department)
        {
            var procedureName = "SP_UpdateDepartment";
            parameters.Add("@Name", department.Name);
            parameters.Add("@Id", Id);
            var updateDept = connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return updateDept;
        }
    }
}