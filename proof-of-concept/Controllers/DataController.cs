using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace proof_of_concept.Controllers
{
    public class EntryController : ApiController
    {
        public struct postStruct
        {
            public String password;
            public int id;
            public String data;
        }

        // GET api/values
        [HttpPost]
        public void AddData(postStruct p)
        {
            
            String password = p.password;
            int id = p.id;
            System.Diagnostics.Debug.WriteLine("password: " + password + "\nid: " + id.ToString());
            object storedPassword = DBHelper.ExecuteScalar("select password from devices where id = " + id.ToString(), new Dictionary<string, object>());
            if(password.Equals((String)storedPassword))
            {
                //TODO accept data
                Dictionary<String, Object> dic = new Dictionary<string, object>();
                dic.Add("@device", id);
                dic.Add("@data", p.data);
                DBHelper.ExecuteNonQuery("insert into entries(device, data) values(@device, @data)", dic);
            }
        }

    }
}
