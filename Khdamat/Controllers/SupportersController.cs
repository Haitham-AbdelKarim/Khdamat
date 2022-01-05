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
    public class SupportersController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        private readonly ApplicationDbContext _context;

        public SupportersController(ApplicationDbContext context)
        {
            _context = context;
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult supportersControl(string id)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID, Admin_b, S_Blocked,Supporter_b FROM Worker, Account WHERE Email = Supporter_Email;";
            dr = com.ExecuteReader();
            List<SupportersDetails> supporters = new List<SupportersDetails>();
            SupportersDetails supportersDetails;
            while (dr.Read())
            {
                supportersDetails = new SupportersDetails();
                supportersDetails.supporter.Natoinal_ID = dr["Natoinal_ID"].ToString();
                supportersDetails.supporter.Client_Email = dr["Worker_Email"].ToString();
                supportersDetails.supporter.First_Name = dr["F_Name"].ToString();
                supportersDetails.supporter.Last_Name = dr["L_Name"].ToString();
                supportersDetails.supporter.Country = dr["Country"].ToString();
                supportersDetails.supporter.City = dr["City"].ToString();
                supportersDetails.supporter.Street = dr["Street"].ToString();
                supportersDetails.supporter.Phone = dr["Phone"].ToString();
                supportersDetails.supporter.Gender = dr["Gender"].ToString()[0];
                supportersDetails.supporter.Birth_Date = ((DateTime)dr["Birth_Date"]);
                //supportersDetails.supporter.r = float.Parse(dr["Rating"].ToString());
                if (dr["Admin_b"].ToString() == "True")
                    supportersDetails.isAdmin = true;
                else
                    supportersDetails.isAdmin = false;

                if (dr["S_Blocked"].ToString() == "True")
                    supportersDetails.isBlocked = true;
                else
                    supportersDetails.isBlocked = false;
                if (string.IsNullOrEmpty(dr["Supporter_b"].ToString()))
                {
                    supportersDetails.isSupporter = false;
                }
                else if (dr["Supporter_b"].ToString() == "True")
                    supportersDetails.isSupporter = true;
                else supportersDetails.isSupporter = false;
                supporters.Add(supportersDetails);
            }
            dr.Close();
            con.Close();
            return View(supporters);
        }

        public IActionResult BlockWorker(Worker c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET S_Blocked= '" + true + "' WHERE Email='" + c.Client_Email + "';";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Rating, Admin_b, S_Blocked,Supporter_b FROM Worker, Account WHERE Email = Worker_Email;";
            dr = com.ExecuteReader();
            List<WorkerDetails> workers = new List<WorkerDetails>();
            WorkerDetails workersDetails;
            while (dr.Read())
            {
                workersDetails = new WorkerDetails();
                workersDetails.worker.Natoinal_ID = dr["Natoinal_ID"].ToString();
                workersDetails.worker.Client_Email = dr["Worker_Email"].ToString();
                workersDetails.worker.First_Name = dr["F_Name"].ToString();
                workersDetails.worker.Last_Name = dr["L_Name"].ToString();
                workersDetails.worker.Country = dr["Country"].ToString();
                workersDetails.worker.City = dr["City"].ToString();
                workersDetails.worker.Street = dr["Street"].ToString();
                workersDetails.worker.Phone = dr["Phone"].ToString();
                workersDetails.worker.Gender = dr["Gender"].ToString()[0];
                workersDetails.worker.Birth_Date = ((DateTime)dr["Birth_Date"]);
                workersDetails.worker.Rating = float.Parse(dr["Rating"].ToString());
                if (dr["Admin_b"].ToString() == "True")
                    workersDetails.isAdmin = true;
                else
                    workersDetails.isAdmin = false;

                if (dr["S_Blocked"].ToString() == "True")
                    workersDetails.isBlocked = true;
                else
                    workersDetails.isBlocked = false;
                if (string.IsNullOrEmpty(dr["Supporter_b"].ToString()))
                {
                    workersDetails.isSupporter = false;
                }
                else if (dr["Supporter_b"].ToString() == "True")
                    workersDetails.isSupporter = true;
                else workersDetails.isSupporter = false;
                workers.Add(workersDetails);
            }
            dr.Close();
            con.Close();
            return View("WorkersControl", workers);
        }

        public IActionResult UNBlockWorker(Worker c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET S_Blocked= '" + false + "' WHERE Email='" + c.Client_Email + "';";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Rating, Admin_b, S_Blocked,Supporter_b FROM Worker, Account WHERE Email = Worker_Email;";
            dr = com.ExecuteReader();
            List<WorkerDetails> workers = new List<WorkerDetails>();
            WorkerDetails workersDetails;
            while (dr.Read())
            {
                workersDetails = new WorkerDetails();
                workersDetails.worker.Natoinal_ID = dr["Natoinal_ID"].ToString();
                workersDetails.worker.Client_Email = dr["Worker_Email"].ToString();
                workersDetails.worker.First_Name = dr["F_Name"].ToString();
                workersDetails.worker.Last_Name = dr["L_Name"].ToString();
                workersDetails.worker.Country = dr["Country"].ToString();
                workersDetails.worker.City = dr["City"].ToString();
                workersDetails.worker.Street = dr["Street"].ToString();
                workersDetails.worker.Phone = dr["Phone"].ToString();
                workersDetails.worker.Gender = dr["Gender"].ToString()[0];
                workersDetails.worker.Birth_Date = ((DateTime)dr["Birth_Date"]);
                workersDetails.worker.Rating = float.Parse(dr["Rating"].ToString());
                if (dr["Admin_b"].ToString() == "True")
                    workersDetails.isAdmin = true;
                else
                    workersDetails.isAdmin = false;

                if (dr["S_Blocked"].ToString() == "True")
                    workersDetails.isBlocked = true;
                else
                    workersDetails.isBlocked = false;
                if (string.IsNullOrEmpty(dr["Supporter_b"].ToString()))
                {
                    workersDetails.isSupporter = false;
                }
                else if (dr["Supporter_b"].ToString() == "True")
                    workersDetails.isSupporter = true;
                else workersDetails.isSupporter = false;
                workers.Add(workersDetails);
            }
            dr.Close();
            con.Close();
            return View("workersControl", workers);
        }

        public IActionResult protosupporter(Worker c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET Supporter_b='" + true + "' WHERE Email='" + c.Client_Email + "';";
            com.ExecuteNonQuery();
            string email = HttpContext.Session.GetString("Email");
            com.CommandText = "SELECT Natoinal_ID FROM Administrator WHERE Admin_Email='" + email + "';";
            dr = com.ExecuteReader();
            dr.Read();
            string id = dr["Natoinal_ID"].ToString();
            dr.Close();

            //com.CommandText = "SELECT Natoinal_ID, Client_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_b, S_Blocked FROM Client, Account WHERE Email = Client_Email;";
            com.CommandText = "INSERT INTO Supporter (Natoinal_ID, Supporter_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID) values ('"
                    + c.Natoinal_ID + "','"
                    + c.Client_Email + "','"
                    + c.First_Name + "','"
                    + c.Last_Name + "','"
                    + c.Country + "','"
                    + c.City + "','"
                    + c.Street + "','"
                    + c.Phone + "','"
                    + c.Gender.ToString() + "','"
                    + c.Birth_Date.ToString("yyyy-MM-dd") + "','"
                    + id
                    + "');";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Rating, Admin_b, S_Blocked,Supporter_b FROM Worker, Account WHERE Email = Worker_Email;";
            dr = com.ExecuteReader();
            List<WorkerDetails> workers = new List<WorkerDetails>();
            WorkerDetails workersDetails;
            while (dr.Read())
            {
                workersDetails = new WorkerDetails();
                workersDetails.worker.Natoinal_ID = dr["Natoinal_ID"].ToString();
                workersDetails.worker.Client_Email = dr["Worker_Email"].ToString();
                workersDetails.worker.First_Name = dr["F_Name"].ToString();
                workersDetails.worker.Last_Name = dr["L_Name"].ToString();
                workersDetails.worker.Country = dr["Country"].ToString();
                workersDetails.worker.City = dr["City"].ToString();
                workersDetails.worker.Street = dr["Street"].ToString();
                workersDetails.worker.Phone = dr["Phone"].ToString();
                workersDetails.worker.Gender = dr["Gender"].ToString()[0];
                workersDetails.worker.Birth_Date = ((DateTime)dr["Birth_Date"]);
                workersDetails.worker.Rating = float.Parse(dr["Rating"].ToString());
                if (dr["Admin_b"].ToString() == "True")
                    workersDetails.isAdmin = true;
                else
                    workersDetails.isAdmin = false;

                if (dr["S_Blocked"].ToString() == "True")
                    workersDetails.isBlocked = true;
                else
                    workersDetails.isBlocked = false;
                if (string.IsNullOrEmpty(dr["Supporter_b"].ToString()))
                {
                    workersDetails.isSupporter = false;
                }
                else if (dr["Supporter_b"].ToString() == "True")
                    workersDetails.isSupporter = true;
                else workersDetails.isSupporter = false;
                workers.Add(workersDetails);
            }
            dr.Close();
            con.Close();
            return View("workersControl", workers);
        }

        
    }
}
