using API.Models;
using API.Repository.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace API.Repository
{
    public class DivisionRepository : IDivisionRepository
    {
        SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString);
        DynamicParameters parameters = new DynamicParameters();

        public int Create(DivisionModel division)
        {
            var procedureName = "SP_InsertDivision";
            parameters.Add("@Name", division.Name);
            parameters.Add("@DepartmentID", division.DepartmentID);
            var create = connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return create;
        }

        public int Delete(int Id)
        {
            var procedureName = "SP_DeleteDivision";
            parameters.Add("@Id", Id);
            var delete = connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return delete;
        }

        public IEnumerable<DivisionViewModel> Get()
        {
            var procedureName = "SP_ViewDivision";
            var getAll = connection.Query<DivisionViewModel>(procedureName, commandType: CommandType.StoredProcedure);
            return getAll;
        }

        public async Task<IEnumerable<DivisionViewModel>> Get(int Id)
        {
            var procedureName = "SP_GetByIdDivision";
            parameters.Add("@Id", Id);
            var getById = await connection.QueryAsync<DivisionViewModel>(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return getById;
        }

        public int Update(int Id, DivisionModel division)
        {
            var procedureName = "SP_UpdateDivision";
            parameters.Add("@Name", division.Name);
            parameters.Add("@Id", Id);
            parameters.Add("@DepartmentID", division.DepartmentID);
            var update = connection.Execute(procedureName, parameters, commandType: CommandType.StoredProcedure);
            return update;
        }
    }
}