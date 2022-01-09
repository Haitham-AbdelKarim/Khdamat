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
    public class ReportsController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        public ReportsController()
        {
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }

        [HttpGet]
        public IActionResult MakeReport(string Natoinal_ID)
        {
            Report report = new Report();
            report.Worker_ID = Natoinal_ID; 
            return View(report);
        }

        // POST: Reports/MakeReport
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MakeReport([Bind("Report_ID, Client_ID, Worker_ID, Supporter_ID, Description")] Report report, string Worker_ID, int ID)
        {
            if (ModelState.IsValid)
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Report (Description, Req_ID) VALUES ('" + report.Description.ToString() + "', '" + ID + "');";
                com.ExecuteNonQuery();
                com.CommandText = "SELECT MAX(Report_ID) FROM Report;";
                int id = (int)com.ExecuteScalar();
                com.CommandText = "SELECT Natoinal_ID FROM Client WHERE Client_Email = '" + HttpContext.Session.GetString("Email")  +  "';";
                dr = com.ExecuteReader();
                dr.Read();
                string CLient_ID = dr["Natoinal_ID"].ToString();
                dr.Close();
                com.CommandText = "INSERT INTO Make_Report (Report_ID, Client_ID, Worker_ID) VALUES ('" + id + "', '" + CLient_ID + "', '" + report.Worker_ID.ToString() +"');";
                com.ExecuteNonQuery();   
                con.Close();
                ViewBag.Message = string.Format("تم ايصال الريبورت بنجاح سيتم اتخاذ الاجراء اللازم من قبل المسؤلين");
            }
            return RedirectToAction(actionName: "MyRequests", controllerName: "Request");
        }

        public IActionResult reportControl()
        {

            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * From Report;";
            dr = com.ExecuteReader();
            List<Report> reports = new List<Report>();
            Report report;
            while (dr.Read())
            {
                report = new Report();
                report.Req_ID = int.Parse(dr["Req_ID"].ToString());
                report.Report_ID = int.Parse(dr["Report_ID"].ToString());
                report.Supporter_ID = dr["Supporter_ID"].ToString();
                report.Description = dr["Description"].ToString();
                reports.Add(report);
            }
            dr.Close();
            com.CommandText = "SELECT * From Make_Report;";
            dr = com.ExecuteReader();
            int i = 0;
            while (dr.Read())
            {
                reports[i].Worker_ID = dr["Worker_ID"].ToString();
                reports[i].Client_ID = dr["Client_ID"].ToString();
                i++;
            }
            dr.Close();
            foreach(Report rep in reports)
            {
                com.CommandText = "SELECT Comment FROM Apply_Req WHERE Req_ID = '" + rep.Req_ID + "' AND Worker_ID = '" + rep.Worker_ID + "';";
                dr = com.ExecuteReader();
                dr.Read();
                rep.Comment = dr["Comment"].ToString();
                dr.Close();
            }

            con.Close();
            return View(reports);
        }
        
    }
}
