using System;
using System.ComponentModel.DataAnnotations;
using ContosoUniversity.Models;

namespace ContosoUniversity.Models
{
    public class Postcard
    {
        public int PostcardId { get; set; }

        [StringLength(150)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Range(1850, 2015)]
        [Display(Name = "Year")]
        public int? Year { get; set; }
        [StringLength(100)]
        [Display(Name = "Image")]
        public string ImageLink { get; set; }

        [Display(Name = "Series")]
        public int SeriesId { get; set; }
        [Display(Name = "Photographer")]
        public int? PhotographerId { get; set; }

        public virtual Series Series { get; set; }
        public virtual Photographer Photographer { get; set; }
    }
}

