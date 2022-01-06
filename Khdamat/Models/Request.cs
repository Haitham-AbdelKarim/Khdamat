using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Khdamat.Models
{
    public class Request
    {
        [Key]
        [DatabaseGenerated(System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.Identity)]
        public int Req_ID { get; set; }

        //[Required]
        [StringLength(maximumLength: 20)]
        public string Client_ID { get; set; }

        [Required (ErrorMessage ="Invalid Input")]
        [StringLength(maximumLength: 50)]
        public string Title { get; set; }

        [StringLength(maximumLength: 8000)]
        public string Description_Req { get; set; }

        [Required]
        public int Service_ID { get; set; }

        public int Min_Age { get; set; }
        public int Max_Age { get; set; }
        public int Max_Price { get; set; }


        [StringLength(maximumLength: 20)]
        public string Supporter_ID { get; set; }

        public char Gender { get; set; }

        [Required(ErrorMessage = "Invalid Input")]
        public DateTime Date_Req { get; set; }

        public char Status { get; set; }

        [Required(ErrorMessage = "Invalid Input")]
        [StringLength(maximumLength: 200)]
        public string City { get; set; }



    }
}
