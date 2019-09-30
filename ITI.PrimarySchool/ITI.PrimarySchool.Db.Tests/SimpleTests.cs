using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;

namespace ITI.PrimarySchool.Db.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        static readonly string ConnectionString = "Server=.;Database=PrimarySchool;Trusted_Connection=True;";

        [Test]
        public async Task get_teachers()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"select t.FirstName, t.LastName, t.TeacherId
                  from ps.tTeacher t
                  where t.TeacherId <> 0;",
                sqlConnection))
            {
                await sqlConnection.OpenAsync();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (await reader.ReadAsync())
                    {
                        int id = (int)reader["TeacherId"];
                        string firstName = (string)reader["FirstName"];
                        string lastName = (string)reader["LastName"];

                        Console.WriteLine("Id: {0} - first name: {1} - last name: {2}", id, firstName, lastName);
                    }
                }
            }
        }

        [Test]
        public async Task insert_new_teacher()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                await conn.OpenAsync();
                string firstName = GetRandomName();
                string lastName = GetRandomName();
                string cmd = @"insert into ps.tTeacher(FirstName,  LastName)
                                                values(@FirstName, @LastName);";

                using (SqlCommand command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    await command.ExecuteNonQueryAsync();
                }

                cmd = @"select t.TeacherId
                        from ps.tTeacher t
                        where t.FirstName = @FirstName
                          and t.LastName = @LastName;";

                int teacherId;
                using (SqlCommand command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    object id = await command.ExecuteScalarAsync();
                    Assert.That(id, Is.Not.Null);
                    teacherId = (int)id;
                }

                cmd = "delete from ps.tTeacher where TeacherId = @TeacherId;";
                using (SqlCommand command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.AddWithValue("@TeacherId", teacherId);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        [Test]
        public async Task insert_new_teacher_with_dapper()
        {
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string firstName = GetRandomName();
                string lastName = GetRandomName();
                await conn.ExecuteAsync(
                    @"insert into ps.tTeacher(FirstName,  LastName)
                                       values(@FirstName, @LastName);",
                    new { FirstName = firstName, LastName = lastName });

                int teacherId = 0;
                Assert.DoesNotThrowAsync(async () =>
                {
                    teacherId = await conn.ExecuteScalarAsync<int>(
                        @"select t.TeacherId
                          from ps.tTeacher t
                          where t.FirstName = @FirstName
                            and t.LastName = @LastName;",
                        new { FirstName = firstName, LastName = lastName });
                });

                await conn.ExecuteAsync(
                    "delete from ps.tTeacher where TeacherId = @TeacherId;",
                    new { TeacherId = teacherId });
            }
        }

        static string GetRandomName() => string.Format("Test-{0}", Guid.NewGuid().ToString().Substring(0, 16));
    }
}
