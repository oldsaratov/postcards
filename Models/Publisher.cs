using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Resourses;

namespace PostcardsManager.Models
{
    public class Publisher
    {
        public int PublisherId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(ResourceType = typeof (Resources), Name = "Publisher_Name_Name")]
        public string Name { get; set; }

        public virtual ICollection<Series> Series { get; set; }
    }
}