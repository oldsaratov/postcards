using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostcardsManager.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "[[[Name should be defined]]]")]
        [StringLength(200, ErrorMessage = "[[[Publisher Name cannot be longer than 200 characters]]]")]
        [Display(Name = "[[[Publisher Name]]]")]
        public string Name { get; set; }

        public virtual ICollection<Series> Series { get; set; }
    }
}