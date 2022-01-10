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

        public async Task<IActionResult> SuggReq([Bind("ID,Title,Worker_ID,Client_ID,Supporter_ID,description,compORsug")] SuggReq suggreq)
        {
            if (ModelState.IsValid)
            {
                con.Open();
                com.Connection = con;
                string email = HttpContext.Session.GetString("Email");
               

                if (HttpContext.Session.GetInt32("isWorker")==1)
                {
                    com.CommandText="select * from Worker where Worker_Email='"+email+"';";
                    dr=com.ExecuteReader();dr.Read();

                    com.CommandText = "INSERT INTO Complain_Suggestion (Title ,Worker_ID, Descriptions,C_or_S) values ('"+suggreq.Title+"','" +dr["Natoinal_ID"]+"','" +suggreq.description +"','"+ suggreq.compORsug+"');";

                    dr.Close();
                }
                else if (HttpContext.Session.GetInt32("isClient")==1)
                {
                    com.CommandText="select * from Client where Client_Email='"+email+"';";
                    dr=com.ExecuteReader();dr.Read();

                    com.CommandText = "INSERT INTO Complain_Suggestion (Title ,Client_ID, Descriptions,C_or_S) values ('" + suggreq.Title + "','" + dr["Natoinal_ID"]+"','" +suggreq.description +"','"+ suggreq.compORsug+"');";
                    dr.Close();
                }
                com.ExecuteNonQuery();
             
                con.Close();
                return RedirectToAction(actionName: "Index", controllerName: "Home");

            }
            else
            return View (suggreq);
        }
        public IActionResult manage_c_or_s(char t)
        {
            ViewData["c_or_s"] = t;
            HttpContext.Session.SetString("c_or_s",t.ToString());
            con.Open();
            com.Connection = con;
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "manageCompSugg";
            com.Parameters.Add("@t", System.Data.SqlDbType.Char).Value = t;
            //com.CommandText = "SELECT ID,Title,Worker_ID,Client_ID,Descriptions,Supporter_ID FROM Complain_Suggestion WHERE C_or_S='" + t + "' ;";
            dr = com.ExecuteReader();
            com.CommandType = System.Data.CommandType.Text;
            List<SuggReqDetails> complainlist = new List<SuggReqDetails>();
            List<string> u_idlist = new List<string>();
            SuggReqDetails complain;
            while (dr.Read())
            {
                if (string.IsNullOrEmpty(dr["Supporter_ID"].ToString()))
                {
                    complain = new SuggReqDetails();
                    complain.suggreq.ID = int.Parse(dr["ID"].ToString());
                    complain.id = int.Parse(dr["ID"].ToString());
                    complain.suggreq.Title = dr["Title"].ToString();
                    complain.suggreq.description = dr["Descriptions"].ToString();
                    complain.suggreq.compORsug = t;
                    if (!string.IsNullOrEmpty(dr["Worker_ID"].ToString()))
                    {
                        u_idlist.Add(dr["Worker_ID"].ToString());
                        complain.c_or_w = "Worker";
                    }
                    if (!string.IsNullOrEmpty(dr["Client_ID"].ToString()))
                    {
                        u_idlist.Add(dr["Client_ID"].ToString());
                        complain.c_or_w = "Client";
                    }
                    complainlist.Add(complain);
                }
            }
            dr.Close();
            for(int i=0; i<u_idlist.Count; i++)
            {
                com.CommandText = "SELECT F_Name, L_Name, " + complainlist[i].c_or_w.ToString() + "_Email FROM " + complainlist[i].c_or_w.ToString() + " WHERE Natoinal_ID='" + u_idlist[i].ToString() + "';";
                dr = com.ExecuteReader();
                dr.Read();
                complainlist[i].f_name =dr["F_Name"].ToString();
                complainlist[i].l_name = dr["L_Name"].ToString();
                complainlist[i].email = dr[complainlist[i].c_or_w.ToString() + "_Email"].ToString();
                dr.Close() ;

            }
            con.Close();
            return View(complainlist);
        }
        public IActionResult view_c_or_s(SuggReqDetails c)
        {
            c.suggreq.ID = c.id;
            con.Open();
            com.Connection = con;
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "viewCompSugg";
            com.Parameters.Add("@ID", System.Data.SqlDbType.Int).Value = c.id;
            //com.CommandText = "SELECT Title, Descriptions, C_or_S FROM Complain_Suggestion WHERE ID='" + c.id + "';";
            //com.CommandText = "SELECT Natoinal_ID FROM SUPPORTER WHERE Supporter_Email='"+HttpContext.Session.GetString("Email")+"'";
            dr = com.ExecuteReader();
            com.CommandType = System.Data.CommandType.Text;
            dr.Read();
            c.suggreq.Title = dr["Title"].ToString();
            c.suggreq.description = dr["Descriptions"].ToString();
            c.suggreq.compORsug = char.Parse(dr["C_or_S"].ToString());
            dr.Close();
            con.Close();
            //string id = dr["Natoinal_ID"].ToString();
            //dr.Close();
            //com.CommandText = "UPDATE Complain_Suggestion SET Supporter_ID='" + id + "' WHERE ID='" + c.suggreq.ID + "';";
            //com.ExecuteNonQuery();
            //dr.Close();
            //con.Close();

            return View(c);
        }

        public IActionResult take_c_or_s(SuggReqDetails c)
        {
            con.Open();
            com.Connection = con;
            string e = HttpContext.Session.GetString("Email");
            com.CommandText = "SELECT Natoinal_ID FROM SUPPORTER WHERE Supporter_Email='" + HttpContext.Session.GetString("Email") + "'";
            dr = com.ExecuteReader();
            dr.Read();
            string id = dr["Natoinal_ID"].ToString();
            dr.Close();
            com.CommandText = "UPDATE Complain_Suggestion SET Supporter_ID='" + id + "' WHERE ID='" + c.id + "';";
            com.ExecuteNonQuery();
            dr.Close();
            con.Close();
            char t =char.Parse(HttpContext.Session.GetString("c_or_s"));
            HttpContext.Session.Remove("c_or_s");
            if(t == 'c')
                return RedirectToAction("manage_c_or_s", "SuggReq",new {t='c'});
            if(t =='s')
                return RedirectToAction("manage_c_or_s", "SuggReq",new {t='s'});
            return View(c);

        }

        public IActionResult delete_c_or_s(int id)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "DELETE FROM Complain_Suggestion WHERE ID='" + id + "';";
            com.ExecuteNonQuery();
            con.Close();
            char t = char.Parse(HttpContext.Session.GetString("c_or_s"));
            HttpContext.Session.Remove("c_or_s");
            if (t == 'c')
                return RedirectToAction("manage_c_or_s", "SuggReq", new { t = 'c' });
            if (t == 's')
                return RedirectToAction("manage_c_or_s", "SuggReq", new { t = 's' });
            return View();
        }
    }
}