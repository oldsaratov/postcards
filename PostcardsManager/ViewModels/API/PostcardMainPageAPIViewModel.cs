using Newtonsoft.Json;

namespace PostcardsManager.ViewModels
{
    public class PostcardMainPageAPIViewModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("imageFrontUrl")]
        public string ImageFrontUrl { get; set; }
        [JsonProperty("frontTitle")]
        public string FrontTitle { get; set; }
        [JsonProperty("year")]
        public string Year { get; set; }
        [JsonProperty("seriesId")]
        public int? SeriesId { get; set; }
        [JsonProperty("photographerId")]
        public int? PhotographerId { get; set; }
    }
}