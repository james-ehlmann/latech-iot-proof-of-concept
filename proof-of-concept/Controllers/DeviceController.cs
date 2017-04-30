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

        /// <summary>
        /// Gets a username and password that can be used to add data to our application. 
        /// </summary>
        /// <returns>
        /// {password:string, id:int}
        /// </returns>
        [HttpGet]
        public DBHelper.IdentificationStruct Request()
        {
            DBHelper.IdentificationStruct r = new DBHelper.IdentificationStruct();
            String password = generateRandomString();
            object id = DBHelper.ExecuteScalar("insert into device (password) values ('" + password + "'); SELECT CAST(scope_identity() AS int)", new Dictionary<string, object>());
            r.password = password;
            r.id = (int)id;

            return r;
        }
    }
}
