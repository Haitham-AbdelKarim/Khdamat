using System.ComponentModel.DataAnnotations;

namespace Khdamat.Models
{
    public class Review
    {
        [Key]
        public int Review_ID { get; set; }
        public string Client_ID { get; set; }
        public int Rating { get; set; }
        [Required]
        public string Description { get; set; }
        public int Req_ID { get; set; }
        public Review() { }
    }
}
