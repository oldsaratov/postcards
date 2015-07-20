using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resourses;

namespace PostcardsManager.Models
{
    public class Series
    {
        public int SeriesId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Display(ResourceType = typeof (Resources), Name = "Series_Title_Title")]
        public string Title { get; set; }

        [Range(1850, 2015)]
        [Display(ResourceType = typeof (Resources), Name = "Series_Year_Year")]
        public int Year { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Series_PublisherId_Publisher")]
        public int PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}