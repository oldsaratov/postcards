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

        [Display(Name = "[[[Year]]]")]
        [StringLength(50, ErrorMessage = "[[[Year cannot be longer than 150 characters]]]")]
        public string Year { get; set; }

        [StringLength(1000, ErrorMessage = "[[[Description cannot be longer than 200 characters]]]")]
        [Display(Name = "[[[Description]]]")]
        public string Description { get; set; }

        [Display(Name = "[[[Publisher]]]")]
        public int? PublisherId { get; set; }

        public virtual Publisher Publisher { get; set; }
        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}