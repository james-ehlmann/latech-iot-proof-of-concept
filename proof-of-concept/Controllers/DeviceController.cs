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
                    cmd.Dispose();
                    return tbl;
                }
            }
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

        // POST api/values
        public void Post([FromBody]string value)
        {
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
