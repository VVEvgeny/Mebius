using System;
using System.Collections.Generic;
using OracleBases.Database.Models;
using Oracle.DataAccess.Client;

namespace OracleBases.Windows
{
    public class Ora
    {
        public static class OraStrings
        {
            public static readonly string SqlOperDay = "SELECT xdate FROM daystat";
            public static readonly string SqlLastDay = "SELECT MAX(xdate) FROM daylast";

            public static readonly string SqlRegions =
                "SELECT result FROM tune_sys WHERE name_sec='region' AND name_par='KodRegionVEP'";
        }

        private string CreateConnectOra(string username, string pass, string host, int port, string serviceName)
        {
            return "user id=" + username + ";password=" + pass +
                   ";data source=(DESCRIPTION=(ADDRESS=(PROTOCOL=tcp)(HOST=" + host + ")(PORT=" + port +
                   "))(CONNECT_DATA=(SERVICE_NAME=" + serviceName + ")))";
        }

        public List<string> GetFromDb(Connect connect, string comand)
        {
            var list = new List<string>();

            var fields = new List<string>();
            try
            {
                try
                {
                    foreach (string s in (comand.Remove(comand.ToLower().IndexOf("from", StringComparison.Ordinal)))
                        .Remove(0, 6).Trim().Split(','))
                    {
                        fields.Add(s);
                    }
                }
                catch
                {
                    throw new Exception("Error parse command=" + comand);
                }

                var con = new OracleConnection
                {
                    ConnectionString = CreateConnectOra(connect.UserId, connect.Password, connect.Host,
                        connect.Port, connect.ServiceName)
                };

                con.Open();

                var command = con.CreateCommand();
                command.CommandText = comand;

                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    foreach (var s in fields)
                    {
                        list.Add(reader[s].ToString());
                    }
                }

                con.Close();
                con.Dispose();
            }
            catch (Exception e)
            {
                list.Clear();
                list.Add(e.Message);
            }
            return list;
        }
    }
}
