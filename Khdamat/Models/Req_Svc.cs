using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Khdamat.Models
{
    public class Req_Svc
    {
        public Request Request { get; set; }
      
        public List<Service> Services { get; set; }

        public List<Request> Requests { get; set; }

        public Req_Svc()
        {
            Services = new List<Service>();
            
            Request = new Request();
        }
    }
    
}
