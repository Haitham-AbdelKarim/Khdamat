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
            com.CommandText = "SELECT Natoinal_ID, Supporter_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID, Admin_b, S_Blocked,Supporter_b FROM Supporter, Account WHERE Email = Supporter_Email;";
            dr = com.ExecuteReader();
            List<SupportersDetails> supporters = new List<SupportersDetails>();
            SupportersDetails supportersDetails;
            while (dr.Read())
            {
                supportersDetails = new SupportersDetails();
                supportersDetails.supporter.Natoinal_ID = dr["Natoinal_ID"].ToString();
                supportersDetails.supporter.Supporter_Email = dr["Supporter_Email"].ToString();
                supportersDetails.supporter.First_Name = dr["F_Name"].ToString();
                supportersDetails.supporter.Last_Name = dr["L_Name"].ToString();
                supportersDetails.supporter.Country = dr["Country"].ToString();
                supportersDetails.supporter.City = dr["City"].ToString();
                supportersDetails.supporter.Street = dr["Street"].ToString();
                supportersDetails.supporter.Phone = dr["Phone"].ToString();
                supportersDetails.supporter.Gender = dr["Gender"].ToString()[0];
                supportersDetails.supporter.Birth_Date = ((DateTime)dr["Birth_Date"]);
                supportersDetails.supporter.Admin_ID = dr["Admin_ID"].ToString();
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

        public IActionResult BlockSupporter(Supporter c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET S_Blocked= '" + true + "' WHERE Email='" + c.Supporter_Email + "';";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Supporter_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID, Admin_b, S_Blocked,Supporter_b FROM Supporter, Account WHERE Email = Supporter_Email;";
            dr = com.ExecuteReader();
            List<SupportersDetails> supporters = new List<SupportersDetails>();
            SupportersDetails supportersDetails;
            while (dr.Read())
            {
                supportersDetails = new SupportersDetails();
                supportersDetails.supporter.Natoinal_ID = dr["Natoinal_ID"].ToString();
                supportersDetails.supporter.Supporter_Email = dr["Supporter_Email"].ToString();
                supportersDetails.supporter.First_Name = dr["F_Name"].ToString();
                supportersDetails.supporter.Last_Name = dr["L_Name"].ToString();
                supportersDetails.supporter.Country = dr["Country"].ToString();
                supportersDetails.supporter.City = dr["City"].ToString();
                supportersDetails.supporter.Street = dr["Street"].ToString();
                supportersDetails.supporter.Phone = dr["Phone"].ToString();
                supportersDetails.supporter.Gender = dr["Gender"].ToString()[0];
                supportersDetails.supporter.Birth_Date = ((DateTime)dr["Birth_Date"]);
                supportersDetails.supporter.Admin_ID = dr["Admin_ID"].ToString();
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
            return View("supportersControl", supporters);
        }

        public IActionResult UNBlockSupporter(Supporter c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET S_Blocked= '" + false + "' WHERE Email='" + c.Supporter_Email + "';";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Supporter_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID, Admin_b, S_Blocked,Supporter_b FROM Supporter, Account WHERE Email = Supporter_Email;";
            dr = com.ExecuteReader();
            List<SupportersDetails> supporters = new List<SupportersDetails>();
            SupportersDetails supportersDetails;
            while (dr.Read())
            {
                supportersDetails = new SupportersDetails();
                supportersDetails.supporter.Natoinal_ID = dr["Natoinal_ID"].ToString();
                supportersDetails.supporter.Supporter_Email = dr["Supporter_Email"].ToString();
                supportersDetails.supporter.First_Name = dr["F_Name"].ToString();
                supportersDetails.supporter.Last_Name = dr["L_Name"].ToString();
                supportersDetails.supporter.Country = dr["Country"].ToString();
                supportersDetails.supporter.City = dr["City"].ToString();
                supportersDetails.supporter.Street = dr["Street"].ToString();
                supportersDetails.supporter.Phone = dr["Phone"].ToString();
                supportersDetails.supporter.Gender = dr["Gender"].ToString()[0];
                supportersDetails.supporter.Birth_Date = ((DateTime)dr["Birth_Date"]);
                supportersDetails.supporter.Admin_ID = dr["Admin_ID"].ToString();
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
            return View("SupportersControl", supporters);
        }

        public IActionResult protoAdmin(Supporter c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET Admin_b='" + true + "' WHERE Email='" + c.Supporter_Email + "';";
            com.ExecuteNonQuery();
            com.CommandText = "INSERT INTO Administrator (Natoinal_ID, Admin_Email, F_Name, L_Name, Country, City, Street, Phone, Gender) values ('"
                    + c.Natoinal_ID + "','"
                    + c.Supporter_Email + "','"
                    + c.First_Name + "','"
                    + c.Last_Name + "','"
                    + c.Country + "','"
                    + c.City + "','"
                    + c.Street + "','"
                    + c.Phone + "','"
                    + c.Gender.ToString()
                    + "');";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Supporter_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID, Admin_b, S_Blocked,Supporter_b FROM Supporter, Account WHERE Email = Supporter_Email;";
            dr = com.ExecuteReader();
            List<SupportersDetails> supporters = new List<SupportersDetails>();
            SupportersDetails supportersDetails;
            while (dr.Read())
            {
                supportersDetails = new SupportersDetails();
                supportersDetails.supporter.Natoinal_ID = dr["Natoinal_ID"].ToString();
                supportersDetails.supporter.Supporter_Email = dr["Supporter_Email"].ToString();
                supportersDetails.supporter.First_Name = dr["F_Name"].ToString();
                supportersDetails.supporter.Last_Name = dr["L_Name"].ToString();
                supportersDetails.supporter.Country = dr["Country"].ToString();
                supportersDetails.supporter.City = dr["City"].ToString();
                supportersDetails.supporter.Street = dr["Street"].ToString();
                supportersDetails.supporter.Phone = dr["Phone"].ToString();
                supportersDetails.supporter.Gender = dr["Gender"].ToString()[0];
                supportersDetails.supporter.Birth_Date = ((DateTime)dr["Birth_Date"]);
                supportersDetails.supporter.Admin_ID = dr["Admin_ID"].ToString();
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
            return View("SupportersControl", supporters);
        }


        public IActionResult demofromsupporter(Supporter c)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "UPDATE Account SET Supporter_b='" + false + "' WHERE Email='" + c.Supporter_Email + "';";
            com.ExecuteNonQuery();
            com.CommandText = "DELETE FROM Supporter WHERE Natoinal_ID = '" + c.Natoinal_ID.ToString() + "';";
            com.ExecuteNonQuery();
            com.CommandText = "SELECT Natoinal_ID, Supporter_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Admin_ID, Admin_b, S_Blocked,Supporter_b FROM Supporter, Account WHERE Email = Supporter_Email;";
            dr = com.ExecuteReader();
            List<SupportersDetails> supporters = new List<SupportersDetails>();
            SupportersDetails supportersDetails;
            while (dr.Read())
            {
                supportersDetails = new SupportersDetails();
                supportersDetails.supporter.Natoinal_ID = dr["Natoinal_ID"].ToString();
                supportersDetails.supporter.Supporter_Email = dr["Supporter_Email"].ToString();
                supportersDetails.supporter.First_Name = dr["F_Name"].ToString();
                supportersDetails.supporter.Last_Name = dr["L_Name"].ToString();
                supportersDetails.supporter.Country = dr["Country"].ToString();
                supportersDetails.supporter.City = dr["City"].ToString();
                supportersDetails.supporter.Street = dr["Street"].ToString();
                supportersDetails.supporter.Phone = dr["Phone"].ToString();
                supportersDetails.supporter.Gender = dr["Gender"].ToString()[0];
                supportersDetails.supporter.Birth_Date = ((DateTime)dr["Birth_Date"]);
                supportersDetails.supporter.Admin_ID = dr["Admin_ID"].ToString();
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
            return View("SupportersControl", supporters);
        }


    }
}
