using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InternetProviderApp.Models;

namespace InternetProviderApp.Repositories
{
    public static class UsedEquipmentRepository
    {
        public static List<UsedEquipment> GetUsedEquipmentsFor(Contract contract)
        {
            var usedEquipments = new List<UsedEquipment>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM UsedEquipments WHERE ContractNumber = @ContractNumber";

                using (var command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@ContractNumber", System.Data.SqlDbType.BigInt);
                    command.Parameters["@ContractNumber"].Value = contract.Number;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usedEquipments.Add(new UsedEquipment
                            {
                                Contract = contract,
                                Equipment = EquipmentRepository.GetEquipmentById(reader.GetInt32(1)),
                                StartDate = reader.GetDateTime(2),
                            });
                        }
                    }
                }
            }

            return usedEquipments;
        }

        public static void AddUsedEquipment(UsedEquipment usedEquipment)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "INSERT INTO UsedEquipments (ContractNumber, EquipmentId, StartDate) " +
                "VALUES (@ContractNumber, @EquipmentId, @StartDate)";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ContractNumber", System.Data.SqlDbType.BigInt);
                    cmd.Parameters.Add("@EquipmentId", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@StartDate", System.Data.SqlDbType.Date);
                    cmd.Parameters["@ContractNumber"].Value = usedEquipment.Contract.Number;
                    cmd.Parameters["@EquipmentId"].Value = usedEquipment.Equipment.Id;
                    cmd.Parameters["@StartDate"].Value = usedEquipment.StartDate;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUsedEquipment(UsedEquipment usedEquipment)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "UPDATE UsedEquipments SET StartDate = @StartDate " +
               "WHERE ContractNumber = @ContractNumber AND EquipmentId = @EquipmentId";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ContractNumber", System.Data.SqlDbType.BigInt);
                    cmd.Parameters.Add("@EquipmentId", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@StartDate", System.Data.SqlDbType.Date);
                    cmd.Parameters["@ContractNumber"].Value = usedEquipment.Contract.Number;
                    cmd.Parameters["@EquipmentId"].Value = usedEquipment.Equipment.Id;
                    cmd.Parameters["@StartDate"].Value = usedEquipment.StartDate;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
