using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Resourses;

namespace PostcardsManager.Models
{
    public class Photographer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(ResourceType = typeof (Resources), Name = "Photographer_LastName_Last_Name")]
        public string LastName { get; set; }

        [StringLength(1000, ErrorMessageResourceType = typeof (Resources),
            ErrorMessageResourceName = "Photographer_MiddleName_Middle_name_cannot_be_longer_than_50_characters_")]
        [Column("MiddleName")]
        [Display(ResourceType = typeof (Resources), Name = "Photographer_MiddleName_Middle_Name")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(1000, ErrorMessageResourceType = typeof (Resources),
            ErrorMessageResourceName = "Photographer_FirstName_First_name_cannot_be_longer_than_50_characters_")]
        [Column("FirstName")]
        [Display(ResourceType = typeof (Resources), Name = "Photographer_FirstName_First_Name")]
        public string FirstName { get; set; }

        [Display(ResourceType = typeof (Resources), Name = "Photographer_FullName_Full_Name")]
        public string FullName
        {
            get { return string.Join(" ", LastName, MiddleName, FirstName); }
        }

        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}