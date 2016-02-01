﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostcardsManager.Models
{
    public class Postcard
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "[[[Year]]]")]
        [StringLength(50, ErrorMessage = "[[[Year cannot be longer than 50 characters]]]")]
        public string Year { get; set; }

        [Display(Name = "[[[Front Image]]]")]
        public int? ImageFrontId { get; set; }

        [Display(Name = "[[[Back Image]]]")]
        public int? ImageBackId { get; set; }
        
        [Display(Name = "[[[Series]]]")]
        public int? SeriesId { get; set; }

        [Display(Name = "[[[Photographer]]]")]
        public int? PhotographerId { get; set; }

        [StringLength(200, ErrorMessage = "[[[Front Title cannot be longer than 200 characters]]]")]
        [Display(Name = "[[[Front Title]]]")]
        public string FrontTitle { get; set; }

        [StringLength(50, ErrorMessage = "[[[Front title font cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Front title font]]]")]
        public string FrontTitleFont { get; set; }

        [StringLength(50, ErrorMessage = "[[[Front title font color cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Front title font color]]]")]
        public string FrontTitleFontColor { get; set; }

        [StringLength(200, ErrorMessage = "[[[Back Title cannot be longer than 200 characters]]]")]
        [Display(Name = "[[[Back Title]]]")]
        public string BackTitle { get; set; }

        [StringLength(50, ErrorMessage = "[[[Back title font cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Back title font]]]")]
        public string BackTitleFont { get; set; }

        [StringLength(50, ErrorMessage = "[[[Back title font color cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Back title font color]]]")]
        public string BackTitleFontColor { get; set; }

        [StringLength(50, ErrorMessage = "[[[Back type cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Back type]]]")]
        public string BackType { get; set; }

        [StringLength(50, ErrorMessage = "[[[Back title place cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Back title place]]]")]
        public string BackTitlePlace { get; set; }

        [StringLength(3, ErrorMessage = "[[[Number in series cannot be longer than 3 characters]]]"), MaxLength(3)]
        [Display(Name = "[[[Number in series]]]")]
        public string NumberInSeries { get; set; }

        [StringLength(50, ErrorMessage = "[[[Post date cannot be longer than 30 characters]]]")]
        [Display(Name = "[[[Post date]]]")]
        public string PostDate { get; set; }

        [StringLength(150, ErrorMessage = "[[Publish place cannot be longer than 30 characters]]]")]
        [Display(Name = "[[[Publish place]]]")]
        public string PublishPlace { get; set; }

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