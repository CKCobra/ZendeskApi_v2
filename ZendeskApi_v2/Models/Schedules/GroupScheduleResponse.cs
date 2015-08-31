using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZendeskApi_v2.Models.Schedules
{
    public class GroupScheduleResponse : GroupResponseBase
    {
        [JsonProperty("schedules")]
        public IList<Schedule> Schedules { get; set; }
    }
}
