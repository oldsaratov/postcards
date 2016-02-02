using Newtonsoft.Json;

namespace PostcardsManager.ViewModels
{
    public class PublisherAPIViewModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}