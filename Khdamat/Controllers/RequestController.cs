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

        public IActionResult SelectedReq(int id)
        {
            //if (HttpContext.Session.GetString("Email")==null)
            //    return RedirectToAction(actionName: "Login", controllerName: "Accounts");
            //else return RedirectToAction(actionName: "Index", controllerName: "Home");

            con.Open();
            com.Connection=con;
            com.CommandText="select * from Request where Req_ID="+id;
           dr= com.ExecuteReader();
            Request rnew = new Request();
            if (dr.Read())
            {
                rnew.Req_ID=int.Parse(dr["Req_ID"].ToString());
                rnew.Client_ID=dr["Client_ID"].ToString();
                rnew.Title=dr["Title"].ToString();
                rnew.Description_Req=dr["Description_Req"].ToString();
                rnew.Min_Age=int.Parse(dr["Min_Age"].ToString());
                rnew.Max_Age=int.Parse(dr["Max_Age"].ToString());
                rnew.Max_Price=int.Parse(dr["Max_Price"].ToString());
                rnew.Supporter_ID=dr["Supporter_ID"].ToString();
                rnew.Gender=char.Parse(dr["Gender"].ToString());
                rnew.Date_Req=DateTime.Parse(dr["Date_Req"].ToString());
                rnew.Status=char.Parse(dr["Status"].ToString());
                rnew.City=dr["City"].ToString();
            }
            ApplytoReq apreq = new ApplytoReq();
            apreq.Request=rnew;
            apreq.id=rnew.Req_ID;
           
            dr.Close();
            com.CommandText="select * from Apply_Req,Worker where Req_ID="+id+" and Worker_ID=Natoinal_ID and Worker_Email='"+HttpContext.Session.GetString("Email")+"';";
            dr=com.ExecuteReader();
            if (dr.HasRows==true)
            {
                apreq.able=false;

            }
            else apreq.able=true;
            dr.Close();
            con.Close();
            return View(apreq);


        }
        [HttpPost]
        public IActionResult ApplytoRequest(ApplytoReq ap)
        {
            //if (HttpContext.Session.GetString("Email")==null)
            //    return RedirectToAction(actionName: "Login", controllerName: "Accounts");
            string email = HttpContext.Session.GetString("Email");
           // int de = HttpContext.Session.GetInt32("reid");
            con.Open();
            com.Connection=con;
            //com.CommandText="select Natoinal_ID from Worker where Worker_Email='"+email+"';";
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "GetNatID";
            com.Parameters.Add("@Email", System.Data.SqlDbType.VarChar).Value = email;
            dr =com.ExecuteReader(); dr.Read();
            com.Parameters.Clear();
            com.CommandText = "InsertAppReq";
            com.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = ap.id;
            com.Parameters.Add("@workerID ", System.Data.SqlDbType.VarChar).Value = dr["Natoinal_ID"];
            com.Parameters.Add("@comment ", System.Data.SqlDbType.VarChar).Value = ap.description;
            //com.CommandText="insert into Apply_Req(Req_ID,Worker_ID,Comment) values("+ap.id+",'"+dr["Natoinal_ID"]+"','"+ap.description+"');";
            dr.Close();
            com.ExecuteNonQuery();
            com.CommandType = System.Data.CommandType.Text;
            con.Close();
            return RedirectToAction(actionName: "Index", controllerName: "Home");


        }
        [HttpGet]
        public IActionResult WaitingRequests(string comx="" )
        {
            WaitingRequests lists = new WaitingRequests();
            con.Open();
            com.Connection=con;
            if (comx=="")
                com.CommandText= "select * from Request where Status='w' AND Supporter_ID <> 'NULL'";
            else
            {
                com.CommandText=comx;
            }
                dr=com.ExecuteReader();
           
            lists.Requests=new List<Request>();   
            while(dr.Read())
            {
                Request rnew =new  Request();
                rnew.Req_ID=int.Parse(dr["Req_ID"].ToString());
                rnew.Client_ID=dr["Client_ID"].ToString();
                rnew.Title=dr["Title"].ToString();
                rnew.Description_Req=dr["Description_Req"].ToString();
                rnew.Min_Age=int.Parse(dr["Min_Age"].ToString());
                rnew.Max_Age=int.Parse(dr["Max_Age"].ToString());
                rnew.Max_Price=int.Parse(dr["Max_Price"].ToString());
                rnew.Supporter_ID=dr["Supporter_ID"].ToString();
                rnew.Gender=char.Parse(dr["Gender"].ToString());
                rnew.Date_Req=DateTime.Parse(dr["Date_Req"].ToString());
                rnew.Status=char.Parse(dr["Status"].ToString());
                rnew.City=dr["City"].ToString();
               lists.Requests.Add(rnew);
            }
            dr.Close();
            com.CommandText="Select Name,Service_ID from Service;";
            dr=com.ExecuteReader();
            while(dr.Read())
            {
                Service s = new Service();
                s.Name=dr["Name"].ToString();
                s.Service_ID=int.Parse(dr["Service_ID"].ToString());
                lists.Services.Add(s);
            }
            dr.Close();
            com.CommandText="select distinct City from Request";
            dr=com.ExecuteReader();
            while(dr.Read())
            {
                string x = dr["City"].ToString();
                lists.cities.Add(x);
               
            }
            dr.Close();
            con.Close();
            return View(lists);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WaitingRequests(WaitingRequests lists)
        {

            string v= "select distinct * from Request as r,Service as s where r.Status='w' and s.Service_ID=r.Service_ID ";
            if (lists.id!=0)
                v+=" and s.Service_ID="+lists.id;
            if(lists.gender!='\0')
                v+=" and r.Gender='"+lists.gender+"'";
            if(lists.loc!=null)
                v+=" and r.City='"+lists.loc+"'";
            if (lists.price!=0)
                v+=" and r.Max_Price>="+lists.price;
            if (lists.id==0&&lists.gender=='\0'&&lists.loc==null&&lists.price==0)
                v="";
            //else v="select * from Request as r,Service as s where r.Status='w' and r.Service_ID=s.Service_ID and s.Service_ID="+lists.id+";";
            return WaitingRequests(v);


        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add_Req(/*[Bind("Request.Title,Request.Min_Age,Request.Max_Age,Request.Max_Price,Request.Gender,Request.Date_Req,Request.Description_Req,Request.Service_ID")]*/ Req_Svc Req_Svc)
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
                com.CommandText = "INSERT INTO Request (Client_ID,Title,Description_Req,Service_ID,Min_Age,Max_Age,Max_Price,Gender,Date_Req,Status,City) values ('"
                    + id + "','"
                    + Req_Svc.Request.Title.ToString() + "','"
                    + des + "','"
                    + Req_Svc.Request.Service_ID + "','"
                    + Req_Svc.Request.Min_Age + "','"
                    + Req_Svc.Request.Max_Age + "','"
                    + Req_Svc.Request.Max_Price + "','"
                    + Req_Svc.Request.Gender.ToString() + "','"
                    + Req_Svc.Request.Date_Req.ToString("yyyy-MM-dd") + "','W','"
                    + Req_Svc.Request.City +"');";
                com.ExecuteNonQuery();
                con.Close();
                ViewBag.Message = string.Format("Request added successfully!");

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
            com.CommandText = "SELECT r.City,r.Supporter_ID, r.Req_ID,r.Title,r.Description_Req,r.Min_Age,r.Max_Age,r.Max_Price,r.Gender,r.Date_Req,c.Client_Email,c.F_Name,c.L_Name,s.Name  FROM Request as r,Client as c,Service as s WHERE c.Natoinal_ID=r.Client_ID AND s.Service_ID=r.Service_ID AND r.Status='w';";
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
                    req.request.Max_Price = int.Parse(dr["Max_Price"].ToString());
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
            com.CommandText = "SELECT r.Req_ID,r.Title,r.Description_Req,r.Min_Age,r.Max_Age,r.Max_Price,r.Gender,r.Date_Req,r.City FROM Request as r WHERE r.Req_ID='" + req.id + "';";
            //com.CommandText = "SELECT Natoinal_ID FROM SUPPORTER WHERE Supporter_Email='"+HttpContext.Session.GetString("Email")+"'";
            dr = com.ExecuteReader();
            dr.Read();
            //req.request.Req_ID = int.Parse(dr["Req_ID"].ToString());
            req.request.Title = dr["Title"].ToString();
            req.request.Description_Req = dr["Description_Req"].ToString();
            req.request.Min_Age = int.Parse(dr["Min_Age"].ToString());
            req.request.Max_Age = int.Parse(dr["Max_Age"].ToString());
            req.request.Max_Price = int.Parse(dr["Max_Price"].ToString());
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

        public IActionResult deletereq(int Req_ID)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "DELETE FROM Request WHERE Req_ID='" + Req_ID + "';";
            com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("MyRequests", "Request");

        }

        public IActionResult deleteApply(int Req_ID, string Worker_ID)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "DELETE FROM Apply_Req WHERE Worker_ID='" + Worker_ID + "' AND Req_ID = '" + Req_ID.ToString() + "';";
            com.ExecuteNonQuery();
            con.Close();

            return RedirectToAction("AppliedRequests", "Request");

        }

        public IActionResult AppliedRequests()
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Request, Apply_Req, Worker WHERE Request.Req_ID = Apply_Req.Req_ID AND Worker_ID = Natoinal_ID AND Worker.Worker_Email = '" + HttpContext.Session.GetString("Email") + "';";
            com.ExecuteNonQuery();
            dr = com.ExecuteReader();
            List<ApplytoReq> requests = new List<ApplytoReq>();
            ApplytoReq applytoReq;
            while (dr.Read())
            {
                applytoReq = new ApplytoReq();
                applytoReq.Request.Req_ID = int.Parse(dr["Req_ID"].ToString());
                applytoReq.Request.Title = dr["Title"].ToString();
                applytoReq.Request.Description_Req = dr["Description_Req"].ToString();
                applytoReq.Request.Min_Age = int.Parse(dr["Min_Age"].ToString());
                applytoReq.Request.Max_Age = int.Parse(dr["Max_Age"].ToString());
                applytoReq.Request.Max_Price = int.Parse(dr["Max_Price"].ToString());
                applytoReq.Request.Gender = char.Parse(dr["Gender"].ToString());
                applytoReq.Request.Date_Req = DateTime.Parse(dr["Date_Req"].ToString());
                applytoReq.Request.City = dr["City"].ToString();
                applytoReq.Request.Status = dr["Status"].ToString()[0];
                applytoReq.description = dr["Comment"].ToString();
                applytoReq.Worker_ID = dr["Worker_ID"].ToString();
                requests.Add(applytoReq);
            }
            dr.Close();
            con.Close();

            return View(requests);

        }

        public IActionResult MyRequests()
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Request, Client WHERE Client_ID = Natoinal_ID AND Client_Email = '" + HttpContext.Session.GetString("Email") + "';";
            com.ExecuteNonQuery();
            dr = com.ExecuteReader();
            List<Request> requests = new List<Request>();
            Request request;
            while (dr.Read())
            {
                request = new Request();
                request.Req_ID = int.Parse(dr["Req_ID"].ToString());
                request.Title = dr["Title"].ToString();
                request.Description_Req = dr["Description_Req"].ToString();
                request.Min_Age = int.Parse(dr["Min_Age"].ToString());
                request.Max_Age = int.Parse(dr["Max_Age"].ToString());
                request.Max_Price = int.Parse(dr["Max_Price"].ToString());
                request.Gender = char.Parse(dr["Gender"].ToString());
                request.Date_Req = DateTime.Parse(dr["Date_Req"].ToString());
                request.City = dr["City"].ToString();
                request.Status = dr["Status"].ToString()[0];
                request.Supporter_ID = dr["Supporter_ID"].ToString();
                requests.Add(request);
            }
            dr.Close();
            con.Close();

            return View(requests);
        }


        public IActionResult RecivedRequests(int Req_ID)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Apply_Req, Worker WHERE Apply_Req.Worker_ID = Worker.Natoinal_ID AND Req_ID = '" + Req_ID + "';";
            com.ExecuteNonQuery();
            dr = com.ExecuteReader();
            List<ApplicationDetails> applicationDetailsList = new List<ApplicationDetails>();
            ApplicationDetails applicationDetails;
            while (dr.Read())
            {
                applicationDetails = new ApplicationDetails();
                applicationDetails.ID = Req_ID;
                applicationDetails.description = dr["Comment"].ToString();
                applicationDetails.worker.Natoinal_ID = dr["Natoinal_ID"].ToString();
                applicationDetails.worker.Client_Email = dr["Worker_Email"].ToString();
                applicationDetails.worker.First_Name = dr["F_Name"].ToString();
                applicationDetails.worker.Last_Name = dr["L_Name"].ToString();
                applicationDetails.worker.Country = dr["Country"].ToString();
                applicationDetails.worker.City = dr["City"].ToString();
                applicationDetails.worker.Phone = dr["Phone"].ToString();
                applicationDetails.worker.Gender = dr["Gender"].ToString()[0];
                applicationDetails.worker.Rating = float.Parse(dr["Rating"].ToString());


                applicationDetailsList.Add(applicationDetails);
            }
            dr.Close();
            con.Close();

            return View(applicationDetailsList);

        }

        public IActionResult acceptApplication(int ID, string Natoinal_ID)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Apply_Req SET Taken = 'True' WHERE Req_ID = '" + ID.ToString() + "' AND Worker_ID = '" + Natoinal_ID.ToString() + "';";
            com.ExecuteNonQuery();
            com.CommandText = "UPDATE Request SET Status = 't' WHERE Req_ID = '" + ID.ToString() + "';";
            com.ExecuteNonQuery();
            con.Close();
            return RedirectToAction("MyRequests", "Request");

        }
    }
}
