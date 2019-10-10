using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace ITI.PrimarySchool.DAL
{
    public class ClassGateway
    {
        public async Task<IEnumerable<ClassData>> GetAll()
        {
            using (SqlConnection conn = new SqlConnection("Server=.;Database=PrimarySchool;Trusted_Connection=True;"))
            {
                return await conn.QueryAsync<ClassData>("select c.ClassId, c.Name, c.[Level] from ps.vClass c;");
            }
        }
    }

    public class ClassData
    {
        public int ClassId { get; set; }

        public string Name { get; set; }

        public string Level { get; set; }
    }
}
