namespace Khdamat.Models
{
    public class ClientDetails
    {
        public Client client { get; set; }
        public bool isAdmin { get; set; }
        public bool isBlocked { get; set; }

        public ClientDetails()
        {
            client = new Client();
        }
    }
}
