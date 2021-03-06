﻿using System;
using System.Data.SQLite;
using FileHelpers;
using System.IO;

namespace VendingMachine.Core
{
    public class DatabaseConnection
    {
        public static bool InitializeDatabase(string fileName)
        {
            VendingMachineProduct[] productArray = new  VendingMachineProduct[40];
            productArray = ReadProductFile(fileName); 
            
            SQLiteConnection.CreateFile("VendingMachineDB.sqlite");
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "CREATE TABLE products (name VARCHAR(20), count VARCHAR(20), price VARCHAR(20), slotNo VARCHAR(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            foreach (VendingMachineProduct product in productArray)
            {
                command.CommandText = "insert into products (name, count, price, slotNo) values (@name, @count, @price, @no)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@name", product.Name));
                command.Parameters.Add(new SQLiteParameter("@count", product.Count));
                command.Parameters.Add(new SQLiteParameter("@price", product.Price));
                command.Parameters.Add(new SQLiteParameter("@no", product.SlotNo));
                command.ExecuteNonQuery();
            }
            sql = "update products set count = TRIM(count);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


            sql = "update products set price = TRIM(price);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "update products set slotNo = TRIM(slotNo);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();

            InitializeMoneyDatabase();
            return true;
        }

        public static void InitializeMoneyDatabase()
        {
            VendingMachineMoney[] productArray = new VendingMachineMoney[40];
            productArray = ReadMoneyFile("money.txt");

            SQLiteConnection.CreateFile("MoneyDB.sqlite");
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=MoneyDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "CREATE TABLE money (moneyType VARCHAR(20), count VARCHAR(20))";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            foreach (VendingMachineMoney product in productArray)
            {
                command.CommandText = "insert into money (moneyType, count) values (@moneyType, @count)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@moneyType", product.MoneyType));
                command.Parameters.Add(new SQLiteParameter("@count", product.Count));
                command.ExecuteNonQuery();
            }
            sql = "update money set count = TRIM(count);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "update money set moneyType = TRIM(moneyType);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
        }

        public static void ListProductsFromDatabase()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "select * from products";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tSlot No: " + reader["slotNo"] + "\tPrice: " + reader["price"] + "\tCount: " + reader["count"]);

            m_dbConnection.Close();
        }

        public static void ListMoneyFromDatabase()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=MoneyDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "select * from money";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Money Type: " + reader["moneyType"] + "\tCount: " + reader["count"]);

            m_dbConnection.Close();
        }

        public static VendingMachineProduct[] ReadProductFile(string fileName)
        {
            var engine = new FileHelperEngine<VendingMachineProduct>();

            var result = engine.ReadFile(fileName);
            
            return result;
        }

        public static VendingMachineMoney[] ReadMoneyFile(string fileName)
        {
            var engine = new FileHelperEngine<VendingMachineMoney>();

            var result = engine.ReadFile(fileName);

            return result;
        }

        public static void UpdateProductDatabase(string[,] productsArray)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "DROP TABLE IF EXISTS products";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE products (name VARCHAR(20), count VARCHAR(20), price VARCHAR(20), slotNo VARCHAR(20))";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            for (int i = 0; i < productsArray.GetLength(0); i++)
            {
                command.CommandText = "insert into products (name, count, price, slotNo) values (@name, @count, @price, @no)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@name", productsArray[i, 0]));
                command.Parameters.Add(new SQLiteParameter("@count", productsArray[i, 1]));
                command.Parameters.Add(new SQLiteParameter("@price", productsArray[i, 2]));
                command.Parameters.Add(new SQLiteParameter("@no", productsArray[i, 3]));
                command.ExecuteNonQuery();
            }

            sql = "update products set count = TRIM(count);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();


            sql = "update products set price = TRIM(price);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "update products set slotNo = TRIM(slotNo);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
            Console.WriteLine("Products database updated.");
        }

        public static void UpdateMoneyDatabase(string[,] moneyArray)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=MoneyDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "DROP TABLE IF EXISTS money";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "CREATE TABLE money (moneyType VARCHAR(20), count VARCHAR(20))";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            for (int i = 0; i < moneyArray.GetLength(0); i++)
            {
                command.CommandText = "insert into money (moneyType, count) values (@moneyType, @count)";
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.Add(new SQLiteParameter("@moneyType", moneyArray[i, 0]));
                command.Parameters.Add(new SQLiteParameter("@count", moneyArray[i, 1]));
                command.ExecuteNonQuery();
            }
            sql = "update money set count = TRIM(count);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            sql = "update money set moneyType = TRIM(moneyType);";
            command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            m_dbConnection.Close();
            Console.WriteLine("Money database updated.");
        }

        public static string[] GetProductInfo(int ProductID)
        {            
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");
            string[] productInfo = new string[4];
            m_dbConnection.Open();
            
            string sql = " ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            command.CommandText = "select * from products where slotNo='" + Convert.ToString(ProductID) + "';";
            command.CommandType = System.Data.CommandType.Text;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                productInfo[0] = Convert.ToString(reader["name"]);
                productInfo[1] = Convert.ToString(reader["count"]);
                productInfo[2] = Convert.ToString(reader["price"]);
                productInfo[3] = Convert.ToString(reader["slotNo"]);
            }

            m_dbConnection.Close();
            return productInfo;
        }

        public static string[,] GetDataFromDB(ref int rowSize, ref int columnSize)
        {
            string[,] tempArray = new string[40, 4];

            if (!File.Exists("VendingMachineDB.sqlite"))
            {
                InitializeDatabase("testtest.txt"); // TODO: MOVE TO DATABASE
                Console.WriteLine("Databases initialized. ");
            }

            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();

            string sql = " ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            command.CommandText = "select * from products;";
            command.CommandType = System.Data.CommandType.Text;
            SQLiteDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {   
                tempArray[i,0] = Convert.ToString(reader["name"]);
                tempArray[i, 1] = Convert.ToString(reader["count"]);
                tempArray[i, 2] = Convert.ToString(reader["price"]);
                tempArray[i, 3] = Convert.ToString(reader["slotNo"]);
                i++;
            }
            rowSize = i;
            columnSize = 4;
            string[,] returnArray = new string[rowSize, columnSize];

            for (int k = 0; k < rowSize; k++)
            {
                for(int l = 0; l<columnSize; l++)
                {
                    returnArray[k, l] = tempArray[k, l];
                }
            }

            m_dbConnection.Close();
            return returnArray;
        }

        public static string[] GetMoneyInfo (int moneyType)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=MoneyDB.sqlite;Version=3;");
            string[] moneyInfo = new string[2];
            m_dbConnection.Open();

            string sql = " ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            command.CommandText = "select * from money where moneyType='" + Convert.ToString(moneyType) + "';";
            command.CommandType = System.Data.CommandType.Text;
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                moneyInfo[0] = Convert.ToString(reader["moneyType"]);
                moneyInfo[1] = Convert.ToString(reader["count"]);
            }

            m_dbConnection.Close();
            return moneyInfo;
        }

    }
}
