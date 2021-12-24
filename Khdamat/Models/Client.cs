using System;
using System.ComponentModel.DataAnnotations;

namespace Khdamat.Models
{
    public class Client
    {
        [Key]
        [StringLength(maximumLength: 20)]
        [Required]
        public string Natoinal_ID { get; set; }
        public string Client_Email { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string First_Name { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Last_Name { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string Country { get; set; }
        [Required]
        [StringLength(maximumLength: 20)]
        public string City { get; set; }
        [StringLength(maximumLength: 50)]
        public string Street { get; set; }
        [Required]
        [StringLength(maximumLength: 15)]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public char Gender { get; set; }
        [Required]
        public DateTime Birth_Date { get; set; }



    }
}
