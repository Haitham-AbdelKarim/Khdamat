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
                com.CommandText = "INSERT INTO Request (Client_ID,Title,Description_Req,Service_ID,Min_Age,Max_Age,Min_Price,Gender,Date_Req,Status,City) values ('"
                    + id + "','"
                    + Req_Svc.Request.Title.ToString() + "','"
                    + des + "','"
                    + Req_Svc.Request.Service_ID + "','"
                    + Req_Svc.Request.Min_Age + "','"
                    + Req_Svc.Request.Max_Age + "','"
                    + Req_Svc.Request.Min_Price + "','"
                    + Req_Svc.Request.Gender.ToString() + "','"
                    + Req_Svc.Request.Date_Req + "','"
                    + 'w' + "','"
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

        public IActionResult managereq()
        {
            
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT r.City,r.Supporter_ID, r.Req_ID,r.Title,r.Description_Req,r.Min_Age,r.Max_Age,r.Min_Price,r.Gender,r.Date_Req,c.Client_Email,c.F_Name,c.L_Name,s.Name  FROM Request as r,Client as c,Service as s WHERE c.Natoinal_ID=r.Client_ID AND s.Service_ID=r.Service_ID AND r.Status='w';";
            dr = com.ExecuteReader();
            List<RequestDetails> reqlist = new List<RequestDetails>();
            RequestDetails req;
            while (dr.Read())
            {
                if (string.IsNullOrEmpty(dr["Supporter_ID"].ToString()))
                {
                    req = new RequestDetails();
                    req.id = int.Parse(dr["Req_ID"].ToString());
                    req.f_name = dr["F_Name"].ToString();
                    req.l_name = dr["L_Name"].ToString();
                    req.email = dr["Client_Email"].ToString();
                    req.sname = dr["Name"].ToString();
                    req.request.Req_ID = int.Parse(dr["Req_ID"].ToString());
                    req.request.Title = dr["Title"].ToString();
                    req.request.Description_Req = dr["Description_Req"].ToString();
                    req.request.Min_Age = int.Parse(dr["Min_Age"].ToString());
                    req.request.Max_Age = int.Parse(dr["Max_Age"].ToString());
                    req.request.Min_Price = int.Parse(dr["Min_Price"].ToString());
                    req.request.Gender =char.Parse(dr["Gender"].ToString());
                    req.request.Date_Req = DateTime.Parse(dr["Date_Req"].ToString());
                    req.request.City = dr["City"].ToString();
                    reqlist.Add(req);
                }
            }
            dr.Close();
            con.Close();
            return View(reqlist);
        }
        public IActionResult viewreq(RequestDetails req)
        {
            req.request.Req_ID = req.id;
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT r.Req_ID,r.Title,r.Description_Req,r.Min_Age,r.Max_Age,r.Min_Price,r.Gender,r.Date_Req,r.City FROM Request as r WHERE r.Req_ID='" + req.id + "';";
            //com.CommandText = "SELECT Natoinal_ID FROM SUPPORTER WHERE Supporter_Email='"+HttpContext.Session.GetString("Email")+"'";
            dr = com.ExecuteReader();
            dr.Read();
            //req.request.Req_ID = int.Parse(dr["Req_ID"].ToString());
            req.request.Title = dr["Title"].ToString();
            req.request.Description_Req = dr["Description_Req"].ToString();
            req.request.Min_Age = int.Parse(dr["Min_Age"].ToString());
            req.request.Max_Age = int.Parse(dr["Max_Age"].ToString());
            req.request.Min_Price = int.Parse(dr["Min_Price"].ToString());
            req.request.Gender = char.Parse(dr["Gender"].ToString());
            req.request.Date_Req = DateTime.Parse(dr["Date_Req"].ToString());
            req.request.City = dr["City"].ToString();

            dr.Close();
            con.Close();
            //string id = dr["Natoinal_ID"].ToString();
            //dr.Close();
            //com.CommandText = "UPDATE Complain_Suggestion SET Supporter_ID='" + id + "' WHERE ID='" + c.suggreq.ID + "';";
            //com.ExecuteNonQuery();
            //dr.Close();
            //con.Close();

            return View(req);
        }

        public IActionResult acceptreq(SuggReqDetails c)
        {
            con.Open();
            com.Connection = con;
            string e = HttpContext.Session.GetString("Email");
            com.CommandText = "SELECT Natoinal_ID FROM SUPPORTER WHERE Supporter_Email='" + HttpContext.Session.GetString("Email") + "'";
            dr = com.ExecuteReader();
            dr.Read();
            string id = dr["Natoinal_ID"].ToString();
            dr.Close();
            com.CommandText = "UPDATE Request SET Supporter_ID='" + id + "' WHERE Req_ID='" + c.id + "';";
            com.ExecuteNonQuery();
            dr.Close();
            con.Close();
            return RedirectToAction("managereq", "Request");

        }

        public IActionResult deletereq(int id)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "DELETE FROM Request WHERE Req_ID='" + id + "';";
            com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("managereq", "Request");

        }

    }
}
