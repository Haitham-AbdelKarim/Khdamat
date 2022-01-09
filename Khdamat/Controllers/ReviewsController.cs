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
    public class ReviewsController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        public ReviewsController()
        {
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }

        [HttpGet]
        public IActionResult MakeReview(int Req_ID)
        {
            Review review = new Review();
            review.Req_ID = Req_ID; 
            return View(review);
        }

        // POST: Reviews/MakeReview
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MakeReview([Bind("Review_ID, Client_ID, Rating, Description")] Review review, int Req_ID)
        {
            if (ModelState.IsValid)
            {
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Review (Description, Rating) VALUES ('" + review.Description.ToString() + "', '" + review.Rating.ToString() + "');";
                com.ExecuteNonQuery();
                com.CommandText = "SELECT MAX(Review_ID) FROM Review;";
                int id = (int)com.ExecuteScalar();
                com.CommandText = "SELECT Natoinal_ID FROM Client WHERE Client_Email = '" + HttpContext.Session.GetString("Email")  +  "';";
                dr = com.ExecuteReader();
                dr.Read();
                string CLient_ID = dr["Natoinal_ID"].ToString();
                dr.Close();
                com.CommandText = "INSERT INTO Make_Review (Review_ID, Client_ID, Req_ID) VALUES ('" + id + "', '" + CLient_ID + "', '" + Req_ID + "');";
                com.ExecuteNonQuery();

                com.CommandText = "UPDATE Request SET Status = 'D' WHERE Req_ID = ' " + Req_ID + "';";
                com.ExecuteNonQuery();

                com.CommandText = "SELECT Worker_ID FROM Apply_Req WHERE Taken = '" + true + "' AND Req_ID = '" + Req_ID + "';";
                dr = com.ExecuteReader();
                dr.Read();
                string Worker_ID = dr["Worker_ID"].ToString();
                dr.Close();
                com.CommandText = "SELECT COUNT(Rating) AS CountOfRatings, SUM(Rating) AS SumOfRatings FROM Request, Apply_Req, Review, Make_Review WHERE Request.Status = 'D' AND Request.Req_ID = Apply_Req.Req_ID AND Apply_Req.Worker_ID = '" + Worker_ID + "' AND Make_Review.Req_ID = Apply_Req.Req_ID AND Review.Review_ID = Make_Review.Review_ID;";
                dr = com.ExecuteReader();
                dr.Read();
                float newRating = (float)(Convert.ToDouble(dr["SumOfRatings"]) / Convert.ToDouble(dr["CountOfRatings"]));
                dr.Close();
                com.CommandText = "UPDATE Worker SET Rating = '" + newRating.ToString() + "' WHERE Natoinal_ID = '" + Worker_ID + "';";
                com.ExecuteNonQuery();
                con.Close();
            }
            return RedirectToAction(actionName: "MyRequests", controllerName: "Request");
        }

        public IActionResult showReviews(string Natoinal_ID)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT Description, Client.F_Name, Client.L_Name FROM Request, Apply_Req, Review, Make_Review, Client WHERE Request.Status = 'D' AND Request.Req_ID = Apply_Req.Req_ID AND Apply_Req.Worker_ID = '" + Natoinal_ID + "' AND Make_Review.Req_ID = Apply_Req.Req_ID AND Review.Review_ID = Make_Review.Review_ID AND Make_Review.Client_ID  = Natoinal_ID;";
            dr = com.ExecuteReader();
            List<Comment> comments = new List<Comment>();
            Comment comment;
            while(dr.Read())
            {
                comment = new Comment();
                comment.Name = dr["F_Name"].ToString() + " " + dr["L_Name"].ToString();
                comment.Description = dr["Description"].ToString();
                comments.Add(comment);
            }
            return View(comments);

        }

    }
}
