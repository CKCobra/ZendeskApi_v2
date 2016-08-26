﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using ZendeskApi_v2.Models.HelpCenter.Translations;

namespace ZendeskApi_v2.Models.Articles
{

    public class Article
    {
        [JsonProperty("id")]
        public long? Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("source_locale")]
        public string SourceLocale { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("html_url")]
        public string HtmlUrl { get; set; }

        [JsonProperty("author_id")]
        public long? AuthorId { get; set; }

        [JsonProperty("comments_disabled", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool CommentsDisabled { get; set; }

        [JsonProperty("outdated", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Outdated { get; set; }

        [JsonProperty("draft", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Draft { get; set; }

        [JsonProperty("label_names")]
        public string[] LabelNames { get; set; }

        [JsonProperty("promoted", DefaultValueHandling = DefaultValueHandling.Include)]
        public bool Promoted { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("vote_sum")]
        public int VoteSum { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        [JsonProperty("section_id")]
        public long? SectionId { get; set; }

        [JsonProperty("created_at")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        [JsonConverter(typeof(IsoDateTimeConverter))]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("translations")]
        public IList<Translation> Translations { get; set; }
    }
}
