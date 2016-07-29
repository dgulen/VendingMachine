using System;
using System.Data.SQLite;
using FileHelpers;
using System.IO;

namespace VendingMachine.Core.DBConnection
{
    public class VendingMachineProductsDB
    {
        public static bool InitializeProductDatabase(string fileName)
        {
            VendingMachineProduct[] productArray = new VendingMachineProduct[40];
            productArray = ReadProductSourceFile(fileName);

            if (!File.Exists("VendingMachineDB.sqlite")) 
            {
                SQLiteConnection.CreateFile("VendingMachineDB.sqlite");
            }
            else
            {
                SQLiteConnection temp_dbConnection;
                temp_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

                temp_dbConnection.Open();
                string temp_sql = "DROP TABLE IF EXISTS products";
                SQLiteCommand temp_command = new SQLiteCommand(temp_sql, temp_dbConnection);
                temp_command.ExecuteNonQuery();

                temp_sql = "DROP TABLE IF EXISTS money";
                temp_command = new SQLiteCommand(temp_sql, temp_dbConnection);
                temp_command.ExecuteNonQuery();

                temp_dbConnection.Close();
                Console.WriteLine("Databases cleared. ");
            }

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

            return true;
        }

        public static void ListProductsFromDatabase()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "select * from products";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            Console.WriteLine("\nVending Machine Product Stock:\n");
            Console.WriteLine( String.Format("|{0,-20}|{1,-10}|{2,-10}|{3,-10}|", "Name", "Price", "Count", "SlotNo"));
            while (reader.Read())
            {
               Console.WriteLine( String.Format("|{0,-20}|{1,-10}|{2,-10}|{3,-10}|", reader["name"], reader["price"], reader["count"], reader["slotNo"]));
                //Console.WriteLine("Name: " + reader["name"] + "\tPrice: " + reader["price"] + "\tCount: " + reader["count"] + "\tSlot No: " + reader["slotNo"]);
            }
            Console.Write("\n");

            m_dbConnection.Close();
        }

        public static VendingMachineProduct[] ReadProductSourceFile(string fileName)
        {
            var engine = new FileHelperEngine<VendingMachineProduct>();

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

        public static string[,] GetDataFromProductsDB(ref int rowSize, ref int columnSize)
        {
            string[,] tempArray = new string[40, 4];

            if (!File.Exists("VendingMachineDB.sqlite"))
            {
                InitializeProductDatabase("testtest.txt"); 
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
                tempArray[i, 0] = Convert.ToString(reader["name"]);
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
                for (int l = 0; l < columnSize; l++)
                {
                    returnArray[k, l] = tempArray[k, l];
                }
            }

            m_dbConnection.Close();
            return returnArray;
        }
    }
}
