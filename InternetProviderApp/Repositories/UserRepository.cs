using InternetProviderApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetProviderApp.Repositories
{
    public static class UserRepository
    {
        public static List<User> GetUsers() 
        {
            var users = new List<User>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Users";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(new User
                            {
                                Username = reader.GetString(0),
                                Password = reader.GetString(1),
                                Role = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            return users;
        }

        public static void AddUser(User user)
        {
            if (user == null) return;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, @Role)";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
                    cmd.Parameters.Add("@Role", System.Data.SqlDbType.NVarChar);

                    cmd.Parameters["@Username"].Value = user.Username;
                    cmd.Parameters["@Password"].Value = user.Password;
                    cmd.Parameters["@Role"].Value = user.Role;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
