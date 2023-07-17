using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace STDAPP
{
    class DB
    {
        MySqlConnection connection = new MySqlConnection("server=88.99.96.4 ;port=3306; username=stardoma_elik; password=Elitos211295; database=stardoma_botkeys1");// Stardom Server

        public void openConnection()

        {
            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();



        }
        public void closeConnection()

        {
            if (connection.State == System.Data.ConnectionState.Open)
                connection.Close();

        }
        public MySqlConnection getConnection()
        {
            return connection;

        }
    }
}
