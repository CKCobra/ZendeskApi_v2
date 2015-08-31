using Newtonsoft.Json;

namespace ZendeskApi_v2.Models.Schedules
{
    public class IndividualScheduleResponse
    {
        [JsonProperty("schedule")]
        public Schedule Schedule { get; set; }
    }
}
