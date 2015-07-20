using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resourses;

namespace PostcardsManager.Models
{
    public class Postcard
    {
        public int PostcardId { get; set; }

        [StringLength(150)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_Title_Title")]
        public string Title { get; set; }

        [Range(1850, 2015)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_Year_Year")]
        public int? Year { get; set; }

        [StringLength(100)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_ImageLinkFront_Image")]
        public string ImageLinkFront { get; set; }

        [StringLength(100)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_ImageLinkBack_Image")]
        public string ImageLinkBack { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_SeriesId_Series")]
        public int? SeriesId { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_PhotographerId_Photographer")]
        public int? PhotographerId { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_FrontTitleFont_Front_title_font")]
        public string FrontTitleFont { get; set; }
        
        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackTitleFont_Back_title_font")]
        public string BackTitleFont { get; set; }
        
        [Display(ResourceType = typeof (Resources), Name = "Postcard_FrontTitleFontColor_Front_title_font_color")]
        public string FrontTitleFontColor { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackTitleFontColor_Back_title_font_color")]
        public string BackTitleFontColor { get; set; }
        
        [Display(ResourceType = typeof (Resources), Name = "Postcard_NumberInSeries_Number_in_series")]
        public string NumberInSeries { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackType_Back_type")]
        public string BackType { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackTitlePlace_Back_title_place")]
        public string BackTitlePlace { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_PostDate_Post_date")]
        public string PostDate { get; set; }

        [NotMapped]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_ImageLinkFront_Image")]
        public string ImageIdFront
        {
            get
            {
                return ImageLinkFront.Substring(ImageLinkFront.IndexOf("/", StringComparison.Ordinal),
                    ImageLinkFront.Length - 1);
            }
        }

        [NotMapped]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_ImageLinkFront_Image")]
        public string ImageIdBack
        {
            get
            {
                return ImageLinkBack.Substring(ImageLinkBack.IndexOf("/", StringComparison.Ordinal),
                    ImageLinkBack.Length - 1);
            }
        }

        public virtual Series Series { get; set; }
        public virtual Photographer Photographer { get; set; }
    }
}