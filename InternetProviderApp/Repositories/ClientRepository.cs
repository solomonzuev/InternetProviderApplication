using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using InternetProviderApp.Models;

namespace InternetProviderApp.Repositories
{
    public static class ClientRepository
    {
        public static Client GetClientById(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Clients WHERE Id = @Id";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Client
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                HomeAddress = reader.GetString(2),
                                PassportNumber = reader.GetString(3),
                                CodPorazdeleniya = reader.GetString(4),
                                DateOfBirth = reader.GetDateTime(5)
                            };
                        }
                    }
                }
            }

            throw new Exception("Клиент с указанным Id не существует в базе данных");
        }

        public static List<Client> GetClients()
        {
            var clients = new List<Client>();
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Clients";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                Id = reader.GetInt32(0),
                                FullName = reader.GetString(1),
                                HomeAddress = reader.GetString(2),
                                PassportNumber = reader.GetString(3),
                                CodPorazdeleniya = reader.GetString(4),
                                DateOfBirth = reader.GetDateTime(5),
                                PhoneNumber = reader["PhoneNumber"] == DBNull.Value ? null : reader.GetString(6),
                                Email = reader["Email"] == DBNull.Value ? null : reader.GetString(7)
                            });
                        }
                    }
                }
            }

            return clients;
        }

        public static void AddUpdateClient(Client client)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string sql;
                if (client.Id == 0)
                {
                    sql = "INSERT INTO Clients (FullName, HomeAddress, PassportNumber, CodPodrazdeleniya, DateOfBirth, PhoneNumber, Email) " +
                        "VALUES (@FullName, @HomeAddress, @PassportNumber, @CodPodrazdeleniya, @DateOfBirth, @PhoneNumber, @Email)";
                }
                else
                {
                    sql = "UPDATE Clients SET FullName = @FullName, HomeAddress = @HomeAddress, " +
                        "PassportNumber = @PassportNumber, CodPodrazdeleniya = @CodPodrazdeleniya," +
                        " DateOfBirth = @DateOfBirth, PhoneNumber = @PhoneNumber, Email = @Email WHERE Id = @Id";
                }

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@FullName", System.Data.SqlDbType.NVarChar);
                    cmd.Parameters.Add("@HomeAddress", System.Data.SqlDbType.NVarChar);
                    cmd.Parameters.Add("@PassportNumber", System.Data.SqlDbType.Char);
                    cmd.Parameters.Add("@CodPodrazdeleniya", System.Data.SqlDbType.Char);
                    cmd.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.Date);
                    cmd.Parameters.Add("@PhoneNumber", System.Data.SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Email", System.Data.SqlDbType.NVarChar);

                    cmd.Parameters["@FullName"].Value = client.FullName;
                    cmd.Parameters["@HomeAddress"].Value = client.HomeAddress;
                    cmd.Parameters["@PassportNumber"].Value = client.PassportNumber;
                    cmd.Parameters["@CodPodrazdeleniya"].Value = client.CodPorazdeleniya;
                    cmd.Parameters["@DateOfBirth"].Value = client.DateOfBirth;
                    
                    // Добавляем номер телефона клиента, который может быть NULL в БД
                    if (client.PhoneNumber == null)
                    {
                        cmd.Parameters["@PhoneNumber"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@PhoneNumber"].Value = client.PhoneNumber;
                    }

                    // Добавляем почту клиента, которая может быть NULL в БД
                    if (client.Email == null)
                    {
                        cmd.Parameters["@Email"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@Email"].Value = client.Email;
                    }

                    if (client.Id != 0)
                    {
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                        cmd.Parameters["@Id"].Value = client.Id;
                    }

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteClients(List<Client> clients)
        {
            if (clients.Count == 0) return;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = new SqlCommand(GetDeleteQuery(clients), connection))
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetDeleteQuery(List<Client> clients)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM Clients WHERE Id IN (");

            foreach (var client in clients)
            {
                sb.Append(client.Id);
                sb.Append(',');
            }

            sb.Length -= 1;
            sb.Append(')');

            return sb.ToString();
        }
    }
}
