namespace Khdamat.Models
{
    public class ApplicationDetails
    {
        public Worker worker { get; set; }
        public int ID { get; set; }
        public string description { get; set; }
        public bool able { get; set; }

        public ApplicationDetails()
        {
            worker = new Worker();
        }
    }
}
