namespace Khdamat.Models
{
    public class AppliedRequests
    {
        public Request request { get; set; }
        public Worker worker { get; set; }

        public AppliedRequests()
            {
                worker = new Worker();
                request = new Request();
            }
    }
}
