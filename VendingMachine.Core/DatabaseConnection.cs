using System;
using System.Data.SQLite;
using FileHelpers;

namespace VendingMachine.Core
{
    public class DatabaseConnection
    {
        public static bool InitializeDatabase()
        {
            VendingMachineProduct[] productArray = new  VendingMachineProduct[40];
            productArray = StartFileHelper();
            
            SQLiteConnection.CreateFile("VendingMachine.sqlite");
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachine.sqlite;Version=3;");

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

            m_dbConnection.Close();

            return true;
        }
        public static void ListProductsFromDatabase()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachine.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "select * from products order by slotNo asc";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
                Console.WriteLine("Name: " + reader["name"] + "\tSlot No: " + reader["slotNo"] + "\tPrice: " + reader["price"]);

            m_dbConnection.Close();

        }

        public static VendingMachineProduct[] StartFileHelper()
        {
            var engine = new FileHelperEngine<VendingMachineProduct>();

            var result = engine.ReadFile("testtest.txt");
            
            return result;
        }
    }
}
