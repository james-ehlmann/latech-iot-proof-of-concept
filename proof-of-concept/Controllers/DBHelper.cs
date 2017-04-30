using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace proof_of_concept.Controllers
{
    public class DBHelper
    {
        public static ConnectionStringSettings Con = ConfigurationManager.ConnectionStrings["myDB"];

        public struct IdentificationStruct
        {
            public String password;
            public int id;
        }

        public static DataTable ExecuteQueryTable(string query, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(Con.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;

                    if (parameters != null)
                    {
                        foreach (string parameter in parameters.Keys)
                        {
                            cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                        }
                    }

                    DataTable tbl = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(tbl);
                    }
                    return tbl;
                }
            }
        }

        public static void ExecuteNonQuery(String query, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(Con.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;

                    if (parameters != null)
                    {
                        foreach (string parameter in parameters.Keys)
                        {
                            cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                        }
                    }
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static object ExecuteScalar(String query, Dictionary<string, object> parameters)
        {
            using (SqlConnection conn = new SqlConnection(Con.ConnectionString))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query;

                    if (parameters != null)
                    {
                        foreach (string parameter in parameters.Keys)
                        {
                            cmd.Parameters.AddWithValue(parameter, parameters[parameter]);
                        }
                    }
                    return cmd.ExecuteScalar();
                }
            }

        }

        public static Boolean CheckPasswd(IdentificationStruct id) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("device", id.id);
            object storedPassword = DBHelper.ExecuteScalar("select password from device where id =  @device", dic);
            return id.password.Equals((String)storedPassword);
        }

        public static void AddUser(IdentificationStruct id, int individual) {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("device", id.id);
            dic.Add("individual", individual);
            if (DBHelper.ExecuteScalar("select device + id as dpid from individual where device = @device and id = @individual", dic) == null)
            {
                DBHelper.ExecuteNonQuery("insert into individual(device, id) values(@device, @individual)", dic);
            }
        }
    }
}