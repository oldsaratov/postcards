using Newtonsoft.Json;
using PostcardsManager.Models;

namespace PostcardsManager.ViewModels
{
    public class SeriesAPIViewModel
    {
        public SeriesAPIViewModel(Series series)
        {
            Id = series.Id;
            Title = series.Title;
            Year = series.Year;
            PublisherId = series.PublisherId;
            Description = series.Description;
        }

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