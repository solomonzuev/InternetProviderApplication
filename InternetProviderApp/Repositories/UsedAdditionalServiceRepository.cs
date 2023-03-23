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
    public static class UsedAdditionalServiceRepository
    {
        public static List<UsedAdditionalService> GetUsedServicesFor(Contract contract)
        {
            var usedServices = new List<UsedAdditionalService>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM UsedServices WHERE ContractNumber = @ContractNumber";
                
                using (var command = new SqlCommand(sql, connection))
                {

                    command.Parameters.Add("@ContractNumber", System.Data.SqlDbType.BigInt);
                    command.Parameters["@ContractNumber"].Value = contract.Number;

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            usedServices.Add(new UsedAdditionalService
                            {
                                Contract = contract,
                                Service = AdditionalServiceRepository.GetAdditionalServiceById(reader.GetInt32(1)),
                                IsServiceWork = reader.GetBoolean(2),
                                DateOfLastPayment = reader.GetDateTime(3),
                            });
                        }
                    }
                }
            }
                
            return usedServices;
        }

        public static void AddUsedAdditionalService(UsedAdditionalService usedService)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "INSERT INTO UsedServices (ContractNumber, ServiceId, IsServiceWork, DateOfLastPayment) " +
                    "VALUES (@ContractNumber, @ServiceId, @IsServiceWork, @DateOfLastPayment)";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ContractNumber", System.Data.SqlDbType.BigInt);
                    cmd.Parameters.Add("@ServiceId", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@IsServiceWork", System.Data.SqlDbType.Bit);
                    cmd.Parameters.Add("@DateOfLastPayment", System.Data.SqlDbType.Date);
                    cmd.Parameters["@ContractNumber"].Value = usedService.Contract.Number;
                    cmd.Parameters["@ServiceId"].Value = usedService.Service.Id;
                    cmd.Parameters["@IsServiceWork"].Value = usedService.IsServiceWork;
                    cmd.Parameters["@DateOfLastPayment"].Value = usedService.DateOfLastPayment;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void UpdateUsedAdditionalService(UsedAdditionalService usedService)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "UPDATE UsedServices SET IsServiceWork = @IsServiceWork, DateOfLastPayment = @DateOfLastPayment " +
                    "WHERE ContractNumber = @ContractNumber AND ServiceId = @ServiceId";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ContractNumber", System.Data.SqlDbType.BigInt);
                    cmd.Parameters.Add("@ServiceId", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@IsServiceWork", System.Data.SqlDbType.Bit);
                    cmd.Parameters.Add("@DateOfLastPayment", System.Data.SqlDbType.Date);
                    cmd.Parameters["@ContractNumber"].Value = usedService.Contract.Number;
                    cmd.Parameters["@ServiceId"].Value = usedService.Service.Id;
                    cmd.Parameters["@IsServiceWork"].Value = usedService.IsServiceWork;
                    cmd.Parameters["@DateOfLastPayment"].Value = usedService.DateOfLastPayment;

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
