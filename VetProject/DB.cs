using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;

namespace VetProject
{
    class DB
    {
        static MySqlConnection conn = new MySqlConnection();
        public static MySqlConnection getConnect()
        {
            try
            {
                string strcon = @"server=localhost;
                                            User ID=root;
                                            password=;
                                            database=veterinary;
                                            Character Set=utf8;";

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.ConnectionString = strcon;
                conn.Open();
                return conn;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
