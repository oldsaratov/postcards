using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PostcardsManager.Models
{
    public class Storage
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "[[[Storage Name should be defined]]]")]
        [StringLength(50, ErrorMessage = "[[[Storage Name cannot be longer than 50 characters]]]")]
        [Display(Name = "[[[Storage Name]]]")]
        public string StorageName { get; set; }
        
        [Required(ErrorMessage = "[[[PublicKey should be defined]]]")]
        [MinLength(13, ErrorMessage = "[[[PublicKey shoud be more then 13 characters]]]")]
        [MaxLength(20, ErrorMessage = "[[[PublicKey shoud be 20 characters]]]")]
        [Display(Name = "[[[PublicKey]]]")]
        public string PublicKey { get; set; }
        
        [Required(ErrorMessage = "[[[PrivateKey should be defined]]]")]
        [MinLength(13, ErrorMessage = "[[[PrivateKey shoud be more then 13 characters]]]")]
        [MaxLength(20, ErrorMessage = "[[[PrivateKey shoud be 20 characters]]]")]
        [Display(Name = "[[[PrivateKey]]]")]
        public string PrivateKey { get; set; }
        
        [Display(Name = "[[[Status]]]")]
        public bool Enabled { get; set; }
        
        public virtual ICollection<Image> Images { get; set; }
    }
}