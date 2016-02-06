using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostcardsManager.Models
{
    public class Photographer
    {
        [Key]
        public int Id { get; set; }
      
        public string LastName { get; set; }
        
        public string MiddleName { get; set; }
        
        [Column("FirstName")]
        public string FirstName { get; set; }
        
        public string FullName
        {
            get { return string.Join(" ", LastName, MiddleName, FirstName); }
        }

        public virtual ICollection<Postcard> Postcards { get; set; }
    }
}