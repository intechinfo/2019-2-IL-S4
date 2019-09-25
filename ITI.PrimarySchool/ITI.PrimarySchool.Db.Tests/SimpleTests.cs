using System;
using System.Data.SqlClient;
using NUnit.Framework;

namespace ITI.PrimarySchool.Db.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        static readonly string ConnectionString = "Server=.;Database=PrimarySchool;Trusted_Connection=True;";

        [Test]
        public void get_teachers()
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            using (SqlCommand command = new SqlCommand(
                @"select t.FirstName, t.LastName, t.TeacherId
                  from ps.tTeacher t
                  where t.TeacherId <> 0;",
                sqlConnection))
            {
                sqlConnection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {

                    while (reader.Read())
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
        public void insert_new_teacher()
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();
                string firstName = GetRandomName();
                string lastName = GetRandomName();
                string cmd = $@"insert into ps.tTeacher(FirstName,  LastName)
                                                 values(@FirstName, @LastName);";

                using(SqlCommand command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    command.ExecuteNonQuery();
                }

                cmd = $@"select t.TeacherId
                         from ps.tTeacher t
                         where t.FirstName = @FirstName
                           and t.LastName = @LastName;";

                int teacherId;
                using (SqlCommand command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.AddWithValue("@FirstName", firstName);
                    command.Parameters.AddWithValue("@LastName", lastName);
                    object id = command.ExecuteScalar();
                    Assert.That(id, Is.Not.Null);
                    teacherId = (int)id;
                }

                cmd = $"delete from ps.tTeacher where TeacherId = @TeacherId;";
                using(SqlCommand command = new SqlCommand(cmd, conn))
                {
                    command.Parameters.AddWithValue("@TeacherId", teacherId);
                    command.ExecuteNonQuery();
                }
            }
        }

        static string GetRandomName() => string.Format("Test-{0}", Guid.NewGuid().ToString().Substring(0, 16));
    }
}
