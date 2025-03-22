using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW_ADO_NET_2
{
    class Program
    {
        private static string connectionString;
        static void Main()
        {
            connectionString = @"Data Source=THERION\SQLEXPRESS;Initial Catalog=master;Integrated Security=True";
            VerifyDatabaseConnection("Storage");
            int input;
            do
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("---------- Storage Database ----------");
                Console.ResetColor();
                Console.WriteLine("Option: 1 - Output item info | 2 - Output available types | 3 - Output available suppliers | 4 - Output items by type | 0 - Exit");
                Console.Write("Input: ");
                input = Convert.ToInt32(Console.ReadLine());

                switch (input)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exiting . . .");
                        Console.ResetColor();
                        break;
                    case 1:
                        Console.Write("Enter item name (e.g. 'Bookshelf'): ");
                        string name = Console.ReadLine();
                        OutputItemInfo(name);
                        break;
                    case 2:
                        OutputTypes();
                        break;
                    case 3:
                        OutputSuppliers();
                        break;
                    case 4:
                        Console.Write("Enter type name (e.g. 'Furniture'): ");
                        string type = Console.ReadLine();
                        OutputByType(type);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Invalid option number");
                        Console.ResetColor();
                        break;
                }
                Console.ReadKey();
                Console.Clear();

            } while (input != 0);

        }
        private static void OutputByType(string type)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"Select Name, Type, Supplier, Price, Number, DateOfSupply from Items where Type = '{type}'";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows) throw new Exception("No items of such type are found");
                            Console.ForegroundColor = ConsoleColor.DarkGray;
                            Console.WriteLine($"\nInquired type: {type}");
                            Console.ResetColor();
                            Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -10} {4, -10} {5, -15}", "Name", "Type", "Supplier", "Price", "Number", "Date of supply");
                            Console.WriteLine(new string('-', 90));
                            while (reader.Read())
                            {
                                Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -10} {4, -10} {5, -15}", reader["Name"], reader["Type"], reader["Supplier"], reader["Price"] + "$", reader["Number"], reader["DateOfSupply"]);
                            }
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
        private static void OutputSuppliers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "\r\nSelect distinct Supplier from Items";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new Exception("No types in catalog");
                            }
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.WriteLine("\n----- Suppliers -----");
                            Console.ResetColor();
                            while (reader.Read())
                            {
                                Console.WriteLine(">" + reader["Supplier"]);
                            }
                            
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
        private static void OutputTypes()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "Select distinct Type from Items";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                throw new Exception("No types in catalog");
                            }
                            Console.ForegroundColor = ConsoleColor.DarkCyan;
                            Console.WriteLine("\n----- Types -----");
                            Console.ResetColor();
                            while (reader.Read())
                            {
                                Console.WriteLine(">" + reader["Type"]);
                            }
                            
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }

        static void OutputItemInfo(string item)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"Select Name, Type, Supplier, Price, Number, DateOfSupply from Items where Name = '{item}'";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(!reader.HasRows) throw new Exception("No such item in catalog");
                            Console.WriteLine("\n{0, -15} {1, -15} {2, -15} {3, -10} {4, -10} {5, -15}", "Name", "Type", "Supplier", "Price", "Number", "Date of supply");
                            Console.WriteLine(new string('-', 90));
                            while(reader.Read())
                            {
                                Console.WriteLine("{0, -15} {1, -15} {2, -15} {3, -10} {4, -10} {5, -15}", reader["Name"], reader["Type"], reader["Supplier"], reader["Price"] + "$", reader["Number"], reader["DateOfSupply"]);
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
        static void VerifyDatabaseConnection(string dbName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"If not exists(Select Name from sys.databases where name = '{dbName}') Create database {dbName}";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Access to the database is verified");
                        Console.ResetColor();
                    }
                }
                connectionString = @"Data Source=THERION\SQLEXPRESS;Initial Catalog=Storage;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "If not exists (Select * from Information_SCHEMA.Tables where Table_Name = 'Items')\r\nCreate table " +
                        "Items\r\n(\r\n\tId int not null identity(1,1),\r\n\tName nvarchar(50) not null check(Name <> ''),\r\n\t" +
                        "Type nvarchar(30) not null check(Type <> ''),\r\n\tSupplier nvarchar(30) not null check(Supplier <> '')," +
                        "\r\n\tPrice money not null,\r\n\tNumber int not null check(Number >= 0),\r\n\tDateOfSupply datetime not null\r\n)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("Access to the tables is verified");
                        Console.ResetColor();
                    }
                }
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(e.Message);
                Console.ResetColor();
            }
        }
        
    }
}
