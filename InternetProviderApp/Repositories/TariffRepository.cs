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
    class TariffRepository
    {
        public static Tariff GetTariffById(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Tariffs WHERE Id = @Id";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Tariff
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PricePerDay = reader.GetDecimal(2),
                                InternetSpeed = reader.GetInt16(3)
                            };
                        }
                    }
                }
            }

            throw new Exception("Тариф с указанным Id не существует в базе данных");
        }

        public static List<Tariff> GetTariffs()
        {
            var tariffs = new List<Tariff>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Tariffs";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tariffs.Add(new Tariff
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                PricePerDay = reader.GetDecimal(2),
                                InternetSpeed = reader.GetInt16(3)
                            });
                        }
                    }
                }
            }

            return tariffs;
        }

        public static void AddUpdateTariff(Tariff tariff)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string sql;
                if (tariff.Id == 0)
                {
                    sql = "INSERT INTO Tariffs (TariffName, PricePerDay, InternetSpeedInMB) VALUES (@TariffName, @PricePerDay, @InternetSpeedInMB)";
                }
                else
                {
                    sql = "UPDATE Tariffs SET TariffName = @TariffName, PricePerDay = @PricePerDay, InternetSpeedInMB = @InternetSpeedInMB " +
                        "WHERE Id = @Id";
                }

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add(new SqlParameter("@TariffName", tariff.Name));
                    cmd.Parameters.Add(new SqlParameter("@PricePerDay", tariff.PricePerDay));
                    cmd.Parameters.Add(new SqlParameter("@InternetSpeedInMB", tariff.InternetSpeed));

                    cmd.Parameters["@TariffName"].SqlDbType = System.Data.SqlDbType.NVarChar;
                    cmd.Parameters["@PricePerDay"].SqlDbType = System.Data.SqlDbType.Decimal;
                    cmd.Parameters["@InternetSpeedInMB"].SqlDbType = System.Data.SqlDbType.Int;

                    if (tariff.Id != 0)
                    {
                        cmd.Parameters.Add(new SqlParameter("@Id", tariff.Id));
                        cmd.Parameters["@Id"].SqlDbType = System.Data.SqlDbType.Int;
                    }

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteTariffs(List<Tariff> tariffs)
        {
            if (tariffs.Count == 0) return;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = new SqlCommand(GetDeleteQuery(tariffs), connection))
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetDeleteQuery(List<Tariff> selectedTariffs)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM Tariffs WHERE Id IN (");

            foreach (var tariff in selectedTariffs)
            {
                sb.Append(tariff.Id);
                sb.Append(',');
            }

            sb.Length -= 1;
            sb.Append(')');

            return sb.ToString();
        }
    }
}
