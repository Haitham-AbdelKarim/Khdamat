using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Khdamat.Data;
using Khdamat.Models;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Http;

namespace Khdamat.Controllers
{
    public class RequestController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        //private readonly ApplicationDbContext _db;
        public RequestController()
        {
            //_db = db;
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }
        [HttpGet]
        public IActionResult Add_Req()
        {
            //Req_Svc req_svc = new Req_Svc();
            
            //ViewBag.servc=_db.Service.ToList();
            //req_svc.SERV= services;

            Req_Svc Req_Svc = new Req_Svc();
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Service;";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                Service s = new Service();
                s.Name = dr["name"].ToString();
                s.Service_ID = int.Parse(dr["Service_ID"].ToString());
                Req_Svc.Services.Add(s);

            }
            con.Close();
            dr.Close();

            return View(Req_Svc);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add_Req(/*[Bind("Request.Title,Request.Min_Age,Request.Max_Age,Request.Min_Price,Request.Gender,Request.Date_Req,Request.Description_Req,Request.Service_ID")]*/ Req_Svc Req_Svc)
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            if (ModelState.IsValid)
            {

                string des;
                if (string.IsNullOrEmpty(Req_Svc.Request.Description_Req))
                    des = "";
                else
                    des = Req_Svc.Request.Description_Req.ToString();
                string email = HttpContext.Session.GetString("Email");
                con.Open();
                com.Connection = con;
                com.CommandText = "SELECT Natoinal_ID FROM Client WHERE Client_Email='" + email + "';";
                dr = com.ExecuteReader();
                string id = "";
                if (dr.Read())
                    id = dr["Natoinal_ID"].ToString();
                con.Close();
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Request (Client_ID,Title,Description_Req,Service_ID,Min_Age,Max_Age,Min_Price,Gender,Date_Req,Done,City) values ('"
                    + id + "','"
                    + Req_Svc.Request.Title.ToString() + "','"
                    + des + "','"
                    + Req_Svc.Request.Service_ID + "','"
                    + Req_Svc.Request.Min_Age + "','"
                    + Req_Svc.Request.Max_Age + "','"
                    + Req_Svc.Request.Min_Price + "','"
                    + Req_Svc.Request.Gender.ToString() + "','"
                    + Req_Svc.Request.Date_Req + "','"
                    + false + "','"
                    + Req_Svc.Request.City +"');";
                com.ExecuteNonQuery();
                con.Close();
                ViewBag.Message = string.Format("Request has been Added Successfully ");

            }
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Service;";
            dr = com.ExecuteReader();
            while (dr.Read())
            {
                Service s = new Service();
                s.Name = dr["name"].ToString();
                s.Service_ID = int.Parse(dr["Service_ID"].ToString());
                Req_Svc.Services.Add(s);

            }
            con.Close();
            dr.Close();


            return View(Req_Svc);
        }
    }
}
