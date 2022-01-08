using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Khdamat.Models
{
   

    public class ApplytoReq
    {
        public Request Request { get; set; }
        public int id { get; set; }
        public string  description { get; set; }
        public bool able { get; set; }
        public ApplytoReq()
        {


            Request = new Request();
            description="";
            id=-1;
        }

        public string Worker_ID { get; set; }

    }
}
