using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostcardsManager.ViewModels
{
    public class PublisherViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "[[[Publisher Name]]]")]
        public string Name { get; set; }

        [Display(Name = "[[[Description]]]")]
        public string Description { get; set; }

        public virtual List<SeriesViewModel> Series { get; set; }
    }
}