using System.ComponentModel.DataAnnotations;

namespace Khdamat.Models
{
    public class Report
    {
        [Key]
        public int Report_ID { get; set; }
        public string Client_ID { get; set; }
        public string Worker_ID { get; set; }
        public string Supporter_ID { get; set; }
        [Required]
        public string Description { get; set; }
        public int Req_ID { get; set; }
        public string Comment { get; set; }

        public Report() { }
    }
}
