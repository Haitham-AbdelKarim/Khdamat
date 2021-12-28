using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Khdamat.Models
{
    public class WaitingRequests
    {
        public string name { get; set; }
        public int id { get; set; }
        public string loc { get; set; }
        public char gender { get; set; }
        public int price { get; set; }
        public List<Service> Services { get; set; }
        public List<Request> Requests { get; set; }
        public List<string> cities { get; set; }
        public WaitingRequests()
        {
            Services = new List<Service>();
            Requests=new List<Request>();
            cities=new List<string>();

        }

    }
}
