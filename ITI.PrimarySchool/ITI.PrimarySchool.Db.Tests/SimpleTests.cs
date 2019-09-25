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
    }
}
