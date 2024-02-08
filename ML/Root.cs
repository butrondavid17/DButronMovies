using Newtonsoft.Json;

namespace ML
{
    public class Root
    {
        [JsonProperty("page")]
        public int Page { get; set; }
        [JsonProperty("results")]
        public List<ML.Result> Results { get; set; }
        [JsonProperty("total_pages")]
        public int TotalPages { get; set; }
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }
    }
}