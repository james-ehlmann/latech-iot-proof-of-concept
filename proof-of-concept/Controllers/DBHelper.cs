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
    }
}