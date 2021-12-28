using System;
using System.ComponentModel.DataAnnotations;
namespace Khdamat.Models
{
    public class Service
    {
        [Key]
        public int Service_ID { get; set; }

        [Required]
        [StringLength(maximumLength: 20)]
        public string Name { get; set; }


    }
}
