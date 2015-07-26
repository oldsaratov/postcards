using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resourses;

namespace PostcardsManager.Models
{
    public class Series
    {
        [Key]
        public int Id { get; set; }

        [StringLength(1000)]
        [Display(Name = "[[[Title]]]")]
        public string Title { get; set; }

        [Range(1850, 2015)]
        [Display(Name = "[[[Year]]]")]
        public int? Year { get; set; }

        [Display(Name = "[[[Publisher]]]")]
        public int? PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}