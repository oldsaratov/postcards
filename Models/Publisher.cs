using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resourses;

namespace PostcardsManager.Models
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Publisher_Name_Name")]
        public string Name { get; set; }

        public virtual ICollection<Series> Series { get; set; }
    }
}