namespace Khdamat.Models
{
    public class SuggReqDetails
    {
        public SuggReq suggreq { get; set; }
        public int id { get; set; }
        public string f_name { get; set;}
        public string l_name { get; set; }
        public string email { get; set; }

        public string c_or_w { get; set; }

        public SuggReqDetails()
        {
            suggreq = new SuggReq();
        }


    }
}
