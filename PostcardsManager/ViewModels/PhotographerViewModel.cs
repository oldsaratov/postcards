using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostcardsManager.ViewModels
{
    public class PhotographerViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "[[[Last name should be defined]]]")]
        [StringLength(50, ErrorMessage = "[[[Last name cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Last Name]]]")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "[[[Middle name cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Middle Name]]]]")]
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "[[[First name should be defined]]]")]
        [StringLength(50, ErrorMessage = "[[[First name cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[First Name]]]")]
        public string FirstName { get; set; }

        [Display(Name = "[[[Full Name]]]")]
        public string FullName
        {
            get { return string.Join(" ", LastName, MiddleName, FirstName); }
        }

        public List<PostcardViewModel> Postcards { get; set; }
    }
}