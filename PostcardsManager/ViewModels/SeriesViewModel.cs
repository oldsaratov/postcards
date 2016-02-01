using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostcardsManager.ViewModels
{
    public class SeriesViewModel
    {
        [JsonProperty("id")]
        public long Id { get; set; }
        
        [StringLength(150, ErrorMessage = "[[[Title cannot be longer than 150 characters]]]")]
        [Display(Name = "[[[Title]]]")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Display(Name = "[[[Year]]]")]
        [JsonProperty("year")]
        [StringLength(50, ErrorMessage = "[[[Year cannot be longer than 150 characters]]]")]
        public string Year { get; set; }

        [Display(Name = "[[[Publisher name]]]")]
        [JsonProperty("publisherName")]
        public string PublisherName { get; set; }

        [StringLength(1000, ErrorMessage = "[[[Description cannot be longer than 200 characters]]]")]
        [Display(Name = "[[[Description]]]")]
        [JsonProperty("description")]
        public string Description { get; set; }


        public List<PostcardMainPageViewModel> Postcards { get; set; }

        public string CoverUrl { get; set; }
    }
}