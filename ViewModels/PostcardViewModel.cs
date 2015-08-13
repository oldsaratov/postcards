using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PostcardsManager.Models;

namespace PostcardsManager.ViewModels
{
    public class PostcardViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "[[[Year]]]")]
        public string Year { get; set; }

        [Display(Name = "[[[Front Image]]]")]
        public string ImageFrontPreviewUrl { get; set; }

        [Display(Name = "[[[Back Image]]]")]
        public string ImageBackPreviewUrl { get; set; }

        public string ImageFrontFullUrl { get; set; }
        public string ImageBackFullUrl { get; set; }

        public int? ImageFrontId { get; set; }
        public int? ImageBackId { get; set; }

        [Display(Name = "[[[Series]]]")]
        public int? SeriesId { get; set; }
        [Display(Name = "[[[Series]]]")]
        public string SeriesTitle { get; set; }

        [Display(Name = "[[[Photographer]]]")]
        public int? PhotographerId { get; set; }
        [Display(Name = "[[[Photographer]]]")]
        public string PhotographerName { get; set; }

        [Display(Name = "[[[Front Title]]]")]
        public string FrontTitle { get; set; }

        [Display(Name = "[[[Front title font]]]")]
        public string FrontTitleFont { get; set; }

        [Display(Name = "[[[Front title font color]]]")]
        public string FrontTitleFontColor { get; set; }

        [Display(Name = "[[[Back Title]]]")]
        public string BackTitle { get; set; }

        [Display(Name = "[[[Back title font]]]")]
        public string BackTitleFont { get; set; }

        [Display(Name = "[[[Back title font color]]]")]
        public string BackTitleFontColor { get; set; }

        [Display(Name = "[[[Back type]]]")]
        public string BackType { get; set; }

        [Display(Name = "[[[Back title place]]]")]
        public string BackTitlePlace { get; set; }

        [Display(Name = "[[[Number in series]]]")]
        public string NumberInSeries { get; set; }

        [Display(Name = "[[[Post date]]]")]
        public string PostDate { get; set; }

        [Display(Name = "[[[Publish place]]]")]
        public string PublishPlace { get; set; }
    }
}