using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using InternetProviderApp.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace InternetProviderApp.Repositories
{
    public static class ContractRepository
    {
        public static List<Contract> GetContracts()
        {
            List<Contract> contracts = new List<Contract>();

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Contracts";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            contracts.Add(new Contract
                            {
                                Number = reader.GetInt64(0),
                                Client = ClientRepository.GetClientById(reader.GetInt32(1)),
                                Tariff = TariffRepository.GetTariffById(reader.GetInt32(2)),
                                Balance = reader.GetDecimal(3),
                                Discount = reader.GetInt16(4),
                                IsTariffWork = reader.GetBoolean(5),
                                DateOfLastPayment = reader.GetDateTime(6)
                            });
                        }
                    }
                }
            }    

            return contracts;
        }

        public static void AddUpdateContract(Contract contract)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string sql;
                if (contract.Number == 0)
                {
                    sql = "INSERT INTO Contracts (ClientId, TariffId, Balance, Discount, IsTariffWork, DateOfLastPayment) " +
                        "VALUES (@ClientId, @TariffId, @Balance, @Discount, @IsTariffWork, @DateOfLastPayment)";
                }
                else
                {
                    sql = "UPDATE Contracts SET ClientId = @ClientId, TariffId = @TariffId, Balance = @Balance, Discount = @Discount, " +
                        "IsTariffWork = @IsTariffWork, DateOfLastPayment = @DateOfLastPayment WHERE Number = @Number";
                }

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@ClientId", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@TariffId", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@Balance", System.Data.SqlDbType.Decimal);
                    cmd.Parameters.Add("@Discount", System.Data.SqlDbType.SmallInt);
                    cmd.Parameters.Add("@IsTariffWork", System.Data.SqlDbType.Bit);
                    cmd.Parameters.Add("@DateOfLastPayment", System.Data.SqlDbType.Date);


                    cmd.Parameters["@ClientId"].Value = contract.Client.Id;
                    cmd.Parameters["@TariffId"].Value = contract.Tariff.Id;
                    cmd.Parameters["@Balance"].Value = contract.Balance;
                    cmd.Parameters["@Discount"].Value = contract.Discount;
                    cmd.Parameters["@IsTariffWork"].Value = contract.IsTariffWork;
                    cmd.Parameters["@DateOfLastPayment"].Value = contract.DateOfLastPayment;

                    if (contract.Number != 0)
                    {
                        cmd.Parameters.Add("@Number", System.Data.SqlDbType.BigInt);
                        cmd.Parameters["@Number"].Value = contract.Number;
                    }

                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void DeleteContracts(List<Contract> contracts)
        {
            if (contracts.Count == 0) return;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = new SqlCommand(GetDeleteQuery(contracts), connection))
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private static string GetDeleteQuery(List<Contract> selectedContracts)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM Contracts WHERE Number IN (");

            foreach (var contract in selectedContracts)
            {
                sb.Append(contract.Number);
                sb.Append(',');
            }

            sb.Length -= 1;
            sb.Append(')');

            return sb.ToString();
        }

        public static Contract GetContractByNumber(long contractNumber)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Contracts WHERE Number = @Number";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@Number", System.Data.SqlDbType.BigInt);
                    cmd.Parameters["@Number"].Value = contractNumber;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Contract
                            {
                                Number = reader.GetInt64(0),
                                Client = ClientRepository.GetClientById(reader.GetInt32(1)),
                                Tariff = TariffRepository.GetTariffById(reader.GetInt32(2)),
                                Balance = reader.GetDecimal(3),
                                Discount = reader.GetInt16(4),
                                IsTariffWork = reader.GetBoolean(5),
                                DateOfLastPayment = reader.GetDateTime(6)
                            };
                        }
                    }
                }
            }

            throw new Exception("Не удалось получить данные контракта");
        }
    }
}
