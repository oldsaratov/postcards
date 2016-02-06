using Newtonsoft.Json;
using PostcardsManager.Models;
using System;
using UploadcareCSharp.Url;

namespace PostcardsManager.ViewModels
{
    public class PostcardAPIViewModel
    {
        public PostcardAPIViewModel()
        {
        }

        public PostcardAPIViewModel(Postcard postcard)
        {
            Id = postcard.Id;
            ImageFrontUrl = Urls.Cdn(new CdnPathBuilder(postcard.ImageFrontUniqId).Resize(360, 226)).OriginalString;
            ImageBackUrl = Urls.Cdn(new CdnPathBuilder(postcard.ImageBackUniqId).Resize(360, 226)).OriginalString;
            FrontTitle = postcard.FrontTitle;
            Year = postcard.Year;
            SeriesId = postcard.SeriesId;
            PhotographerId = postcard.PhotographerId;
            FrontTitleFont = postcard.FrontTitleFont;
            FrontTitleFontColor = postcard.FrontTitleFontColor;
            BackTitle = postcard.BackTitle;
            BackTitleFont = postcard.BackTitleFont;
            BackTitleFontColor = postcard.BackTitleFontColor;
            BackTitlePlace = postcard.BackTitlePlace;
            BackType = postcard.BackType;
            NumberInSeries = postcard.NumberInSeries;
            PostDate = postcard.PostDate;
            PublishPlace = postcard.PublishPlace;
            ImageFrontUniqId = postcard.ImageFrontUniqId;
            ImageBackUniqId = postcard.ImageBackUniqId;
        }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("imageFrontUrl")]
        public string ImageFrontUrl { get; set; }

        [JsonProperty("imageBackUrl")]
        public string ImageBackUrl { get; set; }

        [JsonProperty("frontTitle")]
        public string FrontTitle { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

        [JsonProperty("seriesId")]
        public int? SeriesId { get; set; }

        [JsonProperty("photographerId")]
        public int? PhotographerId { get; set; }

        [JsonProperty("frontTitleFont")]
        public string FrontTitleFont { get; set; }

        [JsonProperty("frontTitleFontColor")]
        public string FrontTitleFontColor { get; set; }

        [JsonProperty("backTitle")]
        public string BackTitle { get; set; }

        [JsonProperty("backTitleFont")]
        public string BackTitleFont { get; set; }

        [JsonProperty("backTitleFontColor")]
        public string BackTitleFontColor { get; set; }

        [JsonProperty("backType")]
        public string BackType { get; set; }

        [JsonProperty("backTitlePlace")]
        public string BackTitlePlace { get; set; }

        [JsonProperty("numberInSeries")]
        public string NumberInSeries { get; set; }

        [JsonProperty("postDate")]
        public string PostDate { get; set; }

        [JsonProperty("publishPlace")]
        public string PublishPlace { get; set; }

        [JsonProperty("imageFrontUniqId")]
        public Guid ImageFrontUniqId { get; set; }

        [JsonProperty("imageBackUniqId")]
        public Guid ImageBackUniqId { get; set; }
    }
}