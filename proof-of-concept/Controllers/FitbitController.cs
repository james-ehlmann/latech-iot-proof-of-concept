using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace proof_of_concept.Controllers
{
    public class FitbitController : ApiController
    {

        public struct FitbitDayStruct
        {
            public DBHelper.IdentificationStruct id;
            public int individual;
            public int happened;
            public int steps;
            public int caloriesOut;
            public float totalDistance;
            public int elevation;
            public float averageHR;
            public int totalMinutesAsleep;
            public int totalSleepRecords;
            public int totalTimeInBed;
        }

        [HttpPost]
        public void AddFitbitDay(FitbitDayStruct d)
        {
            if (DBHelper.CheckPasswd(d.id))
            {
                DBHelper.AddUser(d.id, d.individual);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("device", d.id.id);
                dic.Add("individual", d.individual);
                dic.Add("happened", d.happened);
                dic.Add("steps", d.steps);
                dic.Add("caloriesOut", d.caloriesOut);
                dic.Add("totalDistance", d.totalDistance);
                dic.Add("elevation", d.elevation);
                dic.Add("averageHR", d.averageHR);
                dic.Add("totalMinutesAsleep", d.totalMinutesAsleep);
                dic.Add("totalSleepRecords", d.totalSleepRecords);
                dic.Add("totalTimeInBed", d.totalTimeInBed);
                // check to see if we already have this instance of the object, else insert it. 
                if (DBHelper.ExecuteScalar("select device from fitbit_day where device = @device and individual = @individual and happened = @happened", dic) == null)
                {
                    DBHelper.ExecuteNonQuery("insert into fitbit_day(device, individual, " +
                    "happened, steps, caloriesOut, totalDistance, " +
                    "elevation, averageHR, totalMinutesAsleep, " +
                    "totalSleepRecords, totalTimeInBed) " +
                    "values(@device, @individual, @happened, @steps, @caloriesOut, " +
                    "@totalDistance, @elevation, @averageHR, @totalMinutesAsleep, " +
                    "@totalSleepRecords, @totalTimeInBed)", dic);
                }
            }

        }
    }
}