﻿using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZendeskApi_v2.Models.Schedules
{
    public class GroupScheduleHolidayResponse : GroupResponseBase
    {
        [JsonProperty("holidays")]
        public IList<Holiday> Holidays { get; set; }
    }
}
