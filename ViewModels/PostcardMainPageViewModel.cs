using Newtonsoft.Json;

namespace PostcardsManager.ViewModels
{
    public class PostcardMainPageViewModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        [JsonProperty("imageFrontUrl")]
        public string ImageFrontUrl { get; set; }
        [JsonProperty("frontTitle")]
        public string FrontTitle { get; set; }
    }
}