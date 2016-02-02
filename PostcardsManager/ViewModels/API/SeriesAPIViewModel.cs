using Newtonsoft.Json;

namespace PostcardsManager.ViewModels
{
    public class SeriesAPIViewModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [JsonProperty("title")]
        public string Title { get; set; }
        
        [JsonProperty("year")]
        public string Year { get; set; }
        
        [JsonProperty("publisherId")]
        public int? PublisherId { get; set; }
        
        [JsonProperty("description")]
        public string Description { get; set; }
    }
}