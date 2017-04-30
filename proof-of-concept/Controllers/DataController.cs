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
        public struct PostStruct
        {
           public DBHelper.IdentificationStruct id;
           public String data;
        }
        /// <summary>
        /// Adds Data to the centralized hub anonomized with your anonymous id and password. 
        /// If the id and password do not match an existing case then it still looks like a correct post, that way
        /// no fake data will be entered, and no ones data will be manipulated by an outside source. 
        /// </summary>
        [HttpPost]
        public void AddData(PostStruct p)
        {
            
            String password = p.id.password;
            int id = p.id.id;
            // System.Diagnostics.Debug.WriteLine("password: " + password + "\nid: " + id.ToString());
            if (DBHelper.CheckPasswd(p.id)) { 
                Dictionary<String, Object> dic = new Dictionary<string, object>();
                dic.Add("@device", id);
                dic.Add("@data", p.data);
                DBHelper.ExecuteNonQuery("insert into entries(device, data) values(@device, @data)", dic);
            }
        }
    }
}
