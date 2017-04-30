using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace proof_of_concept.Controllers
{
    public class FridgeController : ApiController
    {
        public struct FridgeDayStruct
        {
            public DBHelper.IdentificationStruct id;
            public int individual;
            public int happened;
            public int caloriesEaten;
        }

        [HttpPost]
        public void AddFridgeDay(FridgeDayStruct d)
        {
            if (DBHelper.CheckPasswd(d.id))
            {
                DBHelper.AddUser(d.id, d.individual);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("device", d.id.id);
                dic.Add("individual", d.individual);
                dic.Add("happened", d.happened);
                dic.Add("caloriesEaten", d.caloriesEaten);
                DBHelper.ExecuteNonQuery("insert into fridge_day(device, individual, happened, caloriesEaten) values(@device, @individual, @happened, @caloriesEaten)", dic);

            }
        }
    }
}