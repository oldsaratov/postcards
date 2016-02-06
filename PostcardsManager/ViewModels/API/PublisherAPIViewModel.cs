using Newtonsoft.Json;
using PostcardsManager.Models;

namespace PostcardsManager.ViewModels
{
    public class PublisherAPIViewModel
    {
        public PublisherAPIViewModel()
        {
        }

        public PublisherAPIViewModel(Publisher publisher)
        {
            Id = publisher.Id;
            Name = publisher.Name;
            Description = publisher.Description;
        }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }
}