using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class AddFavorite
    {
        [JsonProperty("media_type")]
        public string MediaType { get; set; }

        [JsonProperty("media_id")]
        public long MediaId { get; set; }

        [JsonProperty("favorite")]
        public bool Favorite { get; set; }
    }
}
