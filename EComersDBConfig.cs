using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComersDBworkerLib
{
    public  class EComersDBConfig
    {
        string host = "localhost";
        string database = "database";
        string username = "postgres";
        string password = "";

        string connectionString = "";

        public string GetEComersConnectonString()
        {
            connectionString = string.Format("Host = {0}; Database = {1}; Username = {2}; Password = {3}", host, database, username, password);
            return connectionString;
        }

        public void SetConnectionString(string constr)
        {
            connectionString = constr;
        }
    }
}
