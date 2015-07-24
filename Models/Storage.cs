using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostcardsManager.Models
{
    public class Storage
    {
        [Key]
        public int Id { get; set; }

        public string StorageName { get; set; }

        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }

        public int StorageLimit { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public bool IsActive
        {
            get { return Images.Count < StorageLimit - 2; }
        }

        public virtual ICollection<Image> Images { get; set; }
    }
}