using System.Collections.Generic;
using Newtonsoft.Json;

namespace AltNetBot.Models
{
    public class CategoryResult
    {
        [JsonProperty("results")]
        public IList<Category> Results { get; set; }
    }
}