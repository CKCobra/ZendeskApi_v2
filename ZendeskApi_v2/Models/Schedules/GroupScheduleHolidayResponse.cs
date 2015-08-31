using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZendeskApi_v2.Models.Schedules
{
    public class GroupScheduleHolidayResponse
    {
        [JsonProperty("holidays")]
        public IList<Holiday> Holidays { get; set; }
    }
}
