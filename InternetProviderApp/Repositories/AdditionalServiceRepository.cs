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
    public static class AdditionalServiceRepository
    {
        public static List<AdditionalService> GetAdditionalServices()
        {
            var services = new List<AdditionalService>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM AdditionalServices";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new AdditionalService
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PricePerDay = reader.GetDecimal(2),
                                Description = reader["ServiceDesc"] == DBNull.Value ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }

            return services;
        }

        public static AdditionalService GetAdditionalServiceById(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM AdditionalServices WHERE Id = @Id";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AdditionalService
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PricePerDay = reader.GetDecimal(2),
                                Description = reader["ServiceDesc"] == DBNull.Value ? null : reader.GetString(3)
                            };
                        }
                    }
                }
            }

            throw new Exception("Не удалось получить данные о дополнительной услуге");
        }

        public static void AddUpdateAdditionalService(AdditionalService service)
        {
            // Проверить
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string sql;
                if (service.Id == 0)
                {
                    sql = "INSERT INTO AdditionalServices (ServiceName, PricePerDay, ServiceDesc) " +
                        "VALUES (@ServiceName, @PricePerDay, @ServiceDesc)";
                }
                else
                {
                    sql = "UPDATE AdditionalServices SET ServiceName = @ServiceName, PricePerDay = @PricePerDay, " +
                        "ServiceDesc = @ServiceDesc WHERE Id = @Id";
                }

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ServiceName", System.Data.SqlDbType.NVarChar);
                    cmd.Parameters.Add("@PricePerDay", System.Data.SqlDbType.Decimal);
                    cmd.Parameters.Add("@ServiceDesc", System.Data.SqlDbType.NVarChar);

                    cmd.Parameters["@ServiceName"].Value = service.Name;
                    cmd.Parameters["@PricePerDay"].Value = service.PricePerDay;

                    // Добавляем описание услуги, которое может быть NULL в БД
                    if (service.Description == null)
                    {
                        cmd.Parameters["@ServiceDesc"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@ServiceDesc"].Value = service.Description;
                    }

                    if (service.Id != 0)
                    {
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                        cmd.Parameters["@Id"].Value = service.Id;
                    }

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteAdditionalServices(List<AdditionalService> services)
        {
            if (services.Count == 0) return;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = new SqlCommand(GetDeleteQuery(services), connection))
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetDeleteQuery(List<AdditionalService> selectedServices)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM AdditionalServices WHERE Id IN (");

            foreach (var service in selectedServices)
            {
                sb.Append(service.Id);
                sb.Append(',');
            }

            sb.Length -= 1;
            sb.Append(')');

            return sb.ToString();
        }
    }
}
