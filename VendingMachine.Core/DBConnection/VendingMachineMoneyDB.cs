using System;
using System.Data.SQLite;
using FileHelpers;
using System.IO;

namespace VendingMachine.Core.DBConnection
{
    public class VendingMachineMoneyDB
    {
        public static void InitializeMoneyDatabase()
        {
            VendingMachineMoney[] productArray = new VendingMachineMoney[40];
            productArray = ReadMoneySourceFile("money.txt"); // filename

            //SQLiteConnection.CreateFile("VendingMachineDB.sqlite");
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

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

        public static void ListMoneyFromDatabase()
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();
            string sql = "select * from money";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            Console.WriteLine("\nVending Machine Money Stock:\n");
            Console.WriteLine(String.Format("|{0,-20}|{1,-10}|", "Money Type", "Count"));
            while (reader.Read())
            {
                Console.WriteLine(String.Format("|{0,-20}|{1,-10}|", reader["moneyType"], reader["count"]));
              //  Console.WriteLine("Money Type: " + reader["moneyType"] + "\tCount: " + reader["count"]);
            }
            Console.Write("\n");

            m_dbConnection.Close();
        }

        public static VendingMachineMoney[] ReadMoneySourceFile(string fileName)
        {
            var engine = new FileHelperEngine<VendingMachineMoney>();

            var result = engine.ReadFile(fileName);

            return result;
        }

        public static void UpdateMoneyDatabase(string[,] moneyArray)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

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

        public static string[] GetMoneyInfo(int moneyType)
        {
            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");
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

        public static string[,] GetDataFromMoneyDB()
        {
            string[,] tempArray = new string[100, 2];

            if (!File.Exists("VendingMachineDB.sqlite"))
            {
                VendingMachineProductsDB.InitializeProductDatabase("testtest.txt");
                InitializeMoneyDatabase(); // TODO: MOVE TO DATABASE
                Console.WriteLine("Databases initialized. ");
            }

            SQLiteConnection m_dbConnection;
            m_dbConnection = new SQLiteConnection("Data Source=VendingMachineDB.sqlite;Version=3;");

            m_dbConnection.Open();

            string sql = " ";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            command.CommandText = "select * from money;";
            command.CommandType = System.Data.CommandType.Text;
            SQLiteDataReader reader = command.ExecuteReader();
            int i = 0;
            while (reader.Read())
            {
                tempArray[i, 0] = Convert.ToString(reader["moneyType"]);
                tempArray[i, 1] = Convert.ToString(reader["count"]);
                i++;
            }
            int rowSize = i;
            int columnSize = 2;
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
