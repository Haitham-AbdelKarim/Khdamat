namespace Khdamat.Models
{
    public class WorkerDetails
    {
        public Worker worker { get; set; }
        public bool isAdmin { get; set; }
        public bool isBlocked { get; set; }

        public bool isSupporter { get; set; }

        public WorkerDetails()
        {
            worker = new Worker();
        }
    }
}
