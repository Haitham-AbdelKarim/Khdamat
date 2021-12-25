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
using System.Configuration;
using System.IO;

namespace Khdamat.Controllers
{
    public class SuggReqController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        public SuggReqController(ApplicationDbContext context)
        {

            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }
        public IActionResult SuggReq()
        {
            SuggReq sugReq = new SuggReq();
            
            if (HttpContext.Session.GetString("Email")==null)
                return RedirectToAction(actionName: "Login", controllerName: "Accounts");
            sugReq.compORsug='s';
            return View(sugReq);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuggReq([Bind("ID,Worker_ID,Client_ID,Supporter_ID,description,compORsug")] SuggReq suggreq)
        {
            if (ModelState.IsValid)
            {
                con.Open();
                com.Connection = con;
                string query = "SELECT max(ID) from Complain_Suggestion";
                SqlCommand cmd = new SqlCommand(query, con);
                int max = (int)cmd.ExecuteScalar();
                max++;
                string email = HttpContext.Session.GetString("Email");
               

                if (HttpContext.Session.GetInt32("isWorker")==1)
                {
                    com.CommandText="select * from Worker where Worker_Email='"+email+"';";
                    dr=com.ExecuteReader();dr.Read();
                    com.CommandText = "INSERT INTO Complain_Suggestion (ID, Worker_ID, Descriptions,C_or_S) values ("+max+",'" +dr["Natoinal_ID"]+"','" +suggreq.description +"','"+ suggreq.compORsug+"');";
                    dr.Close();
                }
                else if (HttpContext.Session.GetInt32("isClient")==1)
                {
                    com.CommandText="select * from Client where Client_Email='"+email+"';";
                    dr=com.ExecuteReader();dr.Read();
                    com.CommandText = "INSERT INTO Complain_Suggestion (ID, Client_ID, Descriptions,C_or_S) values ("+max+",'" +dr["Natoinal_ID"]+"','" +suggreq.description +"','"+ suggreq.compORsug+"');";
                    dr.Close();
                }
                com.ExecuteNonQuery();
             
                con.Close();
                return RedirectToAction(actionName: "Index", controllerName: "Home");

            }
            else
            return View (suggreq);
        }
    }
}