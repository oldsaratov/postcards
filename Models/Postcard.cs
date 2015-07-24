using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.Ajax.Utilities;
using Resourses;

namespace PostcardsManager.Models
{
    public class Postcard
    {
        [Key]
        public int Id { get; set; }
        
        [Range(1850, 2015)]
        public int? Year { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_ImageLinkFront_Image")]
        public int? ImageFrontId { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_ImageLinkBack_Image")]
        public int? ImageBackId { get; set; }
        
        [Display(ResourceType = typeof (Resources), Name = "Postcard_SeriesId_Series")]
        public int? SeriesId { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Postcard_PhotographerId_Photographer")]
        public int? PhotographerId { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_Title_Title")]
        public string FrontTitle { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_FrontTitleFont_Front_title_font")]
        public string FrontTitleFont { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_FrontTitleFontColor_Front_title_font_color")]
        public string FrontTitleFontColor { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_Title_Title")]
        public string BackTitle { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackTitleFont_Back_title_font")]
        public string BackTitleFont { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackTitleFontColor_Back_title_font_color")]
        public string BackTitleFontColor { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackType_Back_type")]
        public string BackType { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_BackTitlePlace_Back_title_place")]
        public string BackTitlePlace { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_NumberInSeries_Number_in_series")]
        public string NumberInSeries { get; set; }

        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Postcard_PostDate_Post_date")]
        public string PostDate { get; set; }

        [NotMapped]
        public Guid ImageFrontUniqId
        {
            get
            {
                return ImageFront != null ? ImageFront.UniqImageId : new Guid();
            }
        }

        [NotMapped]
        public Guid ImageBackUniqId
        {
            get
            {
                return ImageBack != null ? ImageBack.UniqImageId : new Guid();
            }
        }

        public virtual Image ImageFront { get; set; }
        public virtual Image ImageBack { get; set; }

        public virtual Series Series { get; set; }
        public virtual Photographer Photographer { get; set; }
    }
}