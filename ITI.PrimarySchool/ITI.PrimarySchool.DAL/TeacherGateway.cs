using System;
using System.Collections.Generic;
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
            using(SqlConnection conn = new SqlConnection(_connectionString))
            {
                return await conn.QueryAsync<TeacherData>(
                    @"select t.TeacherId, t.FirstName, t.LastName
                      from ps.vTeacher t;");
            }
        }
    }
}
