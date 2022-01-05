namespace Khdamat.Models
{
    public class RequestDetails
    {
        public Request request { get; set; }
        public int id { get; set; }
        public string f_name { get; set; }
        public string l_name { get; set; }
        public string email { get; set; }

        public string sname { get; set; }

        public RequestDetails()
        {
            request = new Request();
        }
    }
}
