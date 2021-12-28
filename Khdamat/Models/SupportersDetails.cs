namespace Khdamat.Models
{
    public class SupportersDetails
    {
        public Supporter supporter { get; set; }
        public bool isAdmin { get; set; }
        public bool isBlocked { get; set; }

        public bool isSupporter { get; set; }

        public SupportersDetails()
        {
            supporter = new Supporter();
        }
    }
}
