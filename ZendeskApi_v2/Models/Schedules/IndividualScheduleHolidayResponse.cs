using Newtonsoft.Json;

namespace ZendeskApi_v2.Models.Schedules
{
    public class IndividualScheduleHolidayResponse
    {
        [JsonProperty("holiday")]
        public Holiday Holiday { get; set; }
    }
}
