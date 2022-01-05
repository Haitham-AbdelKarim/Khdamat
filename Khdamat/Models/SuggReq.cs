using System;
using System.ComponentModel.DataAnnotations;
namespace Khdamat.Models
{
    public class SuggReq
    {
        [Key]
        [Required]
        public int ID { set; get; }
      
        [StringLength(maximumLength:100)]
        public string Title { set; get; }

        [StringLength(maximumLength: 20)]
        public string Worker_ID { set; get; }

        [StringLength(maximumLength: 20)]
        public string Client_ID { set; get; }

        [StringLength(maximumLength: 20)]
        public string Supporter_ID { set; get; }

        [Required (ErrorMessage ="ادخل الوصف")]
        [StringLength(maximumLength: 1000, ErrorMessage = "ادخل وصف اقصر")]
        public string description { set; get; }

        [Required]
        public char compORsug { set; get; }
    }
}