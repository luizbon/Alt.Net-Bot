﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace AltNetBot.Models
{
    public class Group
    {
        [JsonProperty("lon")]
        public double Longitude { get; set; }

        [JsonProperty("visibility")]
        public string Visibility { get; set; }

        [JsonProperty("organizer")]
        public Organizer Organizer { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("join_mode")]
        public string JoinMode { get; set; }

        [JsonProperty("who")]
        public string Who { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("category")]
        public Category category { get; set; }

        [JsonProperty("topics")]
        public List<Topic> Topics { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }

        [JsonProperty("created")]
        public object Created { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("rating")]
        public double Rating { get; set; }

        [JsonProperty("urlname")]
        public string UrlName { get; set; }

        [JsonProperty("lat")]
        public double Latitude { get; set; }

        [JsonProperty("members")]
        public int Members { get; set; }

        [JsonProperty("group_photo")]
        public GroupPhoto GroupPhoto { get; set; }

        [JsonProperty("group_lon")]
        public double GroupLongitude { get; set; }

        [JsonProperty("group_lat")]
        public double GroupLatitude { get; set; }
    }
}