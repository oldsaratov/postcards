using System;
using System.ComponentModel.DataAnnotations;

namespace PostcardsManager.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public Guid UniqImageId { get; set; }
        public string Url { get; set; }

        public int StorageId { get; set; }

        public virtual Storage Storage { get; set; }
    }
}