using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace ITI.PrimarySchool.DAL
{
    public class TeacherGateway
    {
        readonly string _connectionString;

        public TeacherGateway(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString)) throw new ArgumentException("The connection string must be not null nor whitespace.", nameof(connectionString));
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<TeacherData>> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryAsync<TeacherData>(
                    @"select t.TeacherId, t.FirstName, t.LastName
                      from ps.vTeacher t;");
            }
        }

        public async Task<Result<int>> Create(string firstName, string lastName)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                DynamicParameters parameters = new DynamicParameters(new { FirstName = firstName, LastName = lastName });
                parameters.Add("TeacherId", dbType: DbType.Int32, direction: ParameterDirection.Output);
                parameters.Add("Result", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

                await conn.ExecuteAsync("ps.sTeacherCreate", parameters, commandType: CommandType.StoredProcedure);

                int status = parameters.Get<int>("Result");
                if (status == 1)
                {
                    return Result.CreateError<int>("A teacher with this name already exists.");
                }
                else if (status == 0)
                {
                    int teacherId = parameters.Get<int>("TeacherId");
                    return Result.CreateSuccess(teacherId);
                }

                throw new Exception("Unknown status");
            }
        }
    }
}
