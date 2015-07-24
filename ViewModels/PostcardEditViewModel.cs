using System.ComponentModel.DataAnnotations;
using Resourses;

namespace PostcardsManager.ViewModels
{
    public class PostcardEditViewModel
    {
        [Display(ResourceType = typeof(Resources), Name = "Postcard_Year_Year")]
        public int? Year { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_ImageLinkFront_Image")]
        public string ImageFrontUrl { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_ImageLinkBack_Image")]
        public string ImageBackUrl { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_SeriesId_Series")]
        public int? SeriesId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_PhotographerId_Photographer")]
        public int? PhotographerId { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_Title_Title")]
        public string FrontTitle { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_FrontTitleFont_Front_title_font")]
        public string FrontTitleFont { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_FrontTitleFontColor_Front_title_font_color")]
        public string FrontTitleFontColor { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_Title_Title")]
        public string BackTitle { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_BackTitleFont_Back_title_font")]
        public string BackTitleFont { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_BackTitleFontColor_Back_title_font_color")]
        public string BackTitleFontColor { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_BackType_Back_type")]
        public string BackType { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_BackTitlePlace_Back_title_place")]
        public string BackTitlePlace { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_NumberInSeries_Number_in_series")]
        public string NumberInSeries { get; set; }
        [Display(ResourceType = typeof(Resources), Name = "Postcard_PostDate_Post_date")]
        public string PostDate { get; set; }
    }
}