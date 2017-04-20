using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;

namespace proof_of_concept.Controllers
{
    public class DeviceController : ApiController
    {
        public static ConnectionStringSettings Con = ConfigurationManager.ConnectionStrings["myDB"];

        private DataTable executeQueryTable(string query, Dictionary<string, object> parameters)
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

        private void executeNonQuery(String query, Dictionary<string, object> parameters) {
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

        private object executeScalar(String query, Dictionary<string, object> parameters)
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


        private static String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private String generateRandomString() {

            char[] stringChars = new char[20];
            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }

            return new String(stringChars);
        }

        public struct requestStruct {
            public String password;
            public int id;
        }

        // GET api/values
        public IEnumerable<string> GetAll()
        {
            DataTable ds = executeQueryTable("select * from devices", new Dictionary<String, Object>());
            List<String> results = new List<String>();
            foreach (DataRow i in ds.Rows) {
                results.Add(i["id"].ToString());
            }
            return results;

        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // GET api/values
        [HttpGet]
        public requestStruct Request()
        {
            requestStruct r = new requestStruct();
            String password = generateRandomString();
            object id = executeScalar("insert into devices (password) values ('" + password + "')", new Dictionary<string, object>());
            r.password = password;
            r.id = (int)id;

            return r;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
