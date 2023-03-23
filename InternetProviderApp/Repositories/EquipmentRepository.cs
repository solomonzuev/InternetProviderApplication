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
    public static class EquipmentRepository
    {
        public static List<Equipment> GetEquipments()
        {
            var equipments = new List<Equipment>();
            
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Equipments";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            equipments.Add(new Equipment
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                WarrantyInMonths = reader.GetInt16(3),
                                ImagePreview = reader["ImagePreview"] == DBNull.Value
                                    ? null
                                    : (byte[])reader["ImagePreview"]
                            });
                        }
                    }
                }
            }

            return equipments;
        }

        public static Equipment GetEquipmentById(int id)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                const string sql = "SELECT * FROM Equipments WHERE Id = @Id";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                    cmd.Parameters["@Id"].Value = id;

                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Equipment
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2),
                                WarrantyInMonths = reader.GetInt16(3),
                                ImagePreview = reader["ImagePreview"] == DBNull.Value
                                    ? null
                                    : (byte[])reader["ImagePreview"]
                            };
                        }
                    }
                }
            }

            throw new Exception("Не удалось получить данные оборудования");
        }

        public static void AddUpdateEquipment(Equipment equipment)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                string sql;
                if (equipment.Id == 0)
                {
                    sql = "INSERT INTO Equipments (EquipmentName, Price, WarrantyInMonths, ImagePreview) " +
                        "VALUES (@EquipmentName, @Price, @WarrantyInMonths, @ImagePreview)";
                }
                else
                {
                    sql = "UPDATE Equipments SET EquipmentName = @EquipmentName, Price = @Price, " +
                        "WarrantyInMonths = @WarrantyInMonths, ImagePreview = @ImagePreview " +
                        "WHERE Id = @Id";
                }

                using (var cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.Add("@EquipmentName", System.Data.SqlDbType.NVarChar);
                    cmd.Parameters.Add("@Price", System.Data.SqlDbType.Decimal);
                    cmd.Parameters.Add("@WarrantyInMonths", System.Data.SqlDbType.Int);
                    cmd.Parameters.Add("@ImagePreview", System.Data.SqlDbType.VarBinary);

                    cmd.Parameters["@EquipmentName"].Value = equipment.Name;
                    cmd.Parameters["@Price"].Value = equipment.Price;
                    cmd.Parameters["@WarrantyInMonths"].Value = equipment.WarrantyInMonths;
                    if (equipment.ImagePreview == null)
                    {
                        cmd.Parameters["@ImagePreview"].Value = DBNull.Value;
                    }
                    else
                    {
                        cmd.Parameters["@ImagePreview"].Value = equipment.ImagePreview;
                    }

                    if (equipment.Id != 0)
                    {
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int);
                        cmd.Parameters["@Id"].Value = equipment.Id;
                    }

                    connection.Open();
                    cmd.ExecuteNonQuery();
                } 
            }  
        }

        public static void DeleteEquipments(List<Equipment> equipments)
        {
            if (equipments.Count == 0) return;

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                using (var cmd = new SqlCommand(GetDeleteQuery(equipments), connection))
                {
                    connection.Open();
                    cmd.ExecuteNonQuery();
                }
            }  
        }

        private static string GetDeleteQuery(List<Equipment> selectedEquipments)
        {
            var sb = new StringBuilder();
            sb.Append("DELETE FROM Equipments WHERE Id IN (");

            foreach (var equipment in selectedEquipments)
            {
                sb.Append(equipment.Id);
                sb.Append(',');
            }

            sb.Length -= 1;
            sb.Append(')');

            return sb.ToString();
        }
    }
}
