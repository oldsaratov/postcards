using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostcardsManager.Models
{
    public class Series
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "[[[Title should be defined]]]")]
        [StringLength(150, ErrorMessage = "[[[Publisher Name cannot be longer than 150 characters]]]")]
        [Display(Name = "[[[Title]]]")]
        public string Title { get; set; }

        [Range(1840, 2015)]
        [Display(Name = "[[[Year]]]")]
        public int? Year { get; set; }

        [Display(Name = "[[[Publisher]]]")]
        public int? PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}