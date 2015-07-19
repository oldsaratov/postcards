using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{
    public class Series
    {
        public int SeriesId { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Range(1850, 2015)]
        [Display(Name = "Year")]
        public int Year { get; set; }
        [Display(Name = "Publisher")]
        public int PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}