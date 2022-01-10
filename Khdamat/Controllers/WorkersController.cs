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
    public class WorkersController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        private readonly ApplicationDbContext _context;

        public WorkersController(ApplicationDbContext context)
        {
            _context = context;
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }

        // GET: Workers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Worker.ToListAsync());
        }

        // GET: Workers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .FirstOrDefaultAsync(m => m.Natoinal_ID == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // GET: Workers/Register
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("Email") == null)
                return RedirectToAction("Register", "Accounts");
            if (HttpContext.Session.GetInt32("isWorker") != null)
                return RedirectToAction("Register", "Accounts");
            Worker worker = new Worker();
            return View(worker);
        }

        // POST: Workers/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Natoinal_ID,Worker_Email,First_Name,Last_Name,Country,City,Street,Phone,Gender,Birth_Date")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("Email") == null)
                    return RedirectToAction("Register", "Accounts");
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Worker (Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date) values ('"
                    + worker.Natoinal_ID + "','"
                    + HttpContext.Session.GetString("Email") + "','"
                    + worker.First_Name + "','"
                    + worker.Last_Name + "','"
                    + worker.Country + "','"
                    + worker.City + "','"
                    + worker.Street + "','"
                    + worker.Phone + "','"
                    + worker.Gender.ToString() + "','"
                    + worker.Birth_Date.ToString("yyyy-MM-dd")
                    + "');";
                try
                {
                    com.ExecuteNonQuery();
                    
                }
                catch
                {
                    TempData["AlertMessage"] = "هذا الرقم القومى مسجل من قبل";
                    return View(worker);
                }
                    com.CommandText = "UPDATE ACCOUNT SET Worker_b = '1' WHERE Email = '"
                        + HttpContext.Session.GetString("Email") + "';";
                    com.ExecuteNonQuery();
                    HttpContext.Session.SetInt32("isWorker", 1);
                    HttpContext.Session.SetString("FirstName", worker.First_Name.ToString());
                con.Close();
                return RedirectToAction("Index", "Home");
            }
            return View(worker);
        }


        // GET: Workers/profile
        public IActionResult profile()
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT Natoinal_ID, Worker_Email, F_Name, L_Name, Phone, Gender, Rating FROM Worker, Account WHERE Email = Worker_Email AND Email = '" + HttpContext.Session.GetString("Email") + " ';";
            dr = com.ExecuteReader();
            dr.Read();
            Worker worker = new Worker();
            worker.Natoinal_ID = dr["Natoinal_ID"].ToString();
            worker.First_Name = dr["F_Name"].ToString();
            worker.Last_Name = dr["L_Name"].ToString();
            worker.Client_Email = dr["Worker_Email"].ToString();
            worker.Phone = dr["Phone"].ToString();
            worker.Gender = dr["Gender"].ToString()[0];
            worker.Rating = (float)dr["Rating"].ToString()[0] - 48;
            dr.Close();
            con.Close();

            return View(worker);
        }


        // POST: Workers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Natoinal_ID,Client_Email,First_Name,Last_Name,Country,City,Street,Phone,Gender,Birth_Date,Rating")] Worker worker)
        {
            if (id != worker.Natoinal_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(worker);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkerExists(worker.Natoinal_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(worker);
        }

        // GET: Workers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var worker = await _context.Worker
                .FirstOrDefaultAsync(m => m.Natoinal_ID == id);
            if (worker == null)
            {
                return NotFound();
            }

            return View(worker);
        }

        // POST: Workers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var worker = await _context.Worker.FindAsync(id);
            _context.Worker.Remove(worker);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkerExists(string id)
        {
            return _context.Worker.Any(e => e.Natoinal_ID == id);
        }
        public IActionResult workersControl(string id)
        {
            con.Open();
            com.Connection = con;
            com.CommandType = System.Data.CommandType.StoredProcedure;
            com.CommandText = "WorkerControl";
            //com.CommandText = "SELECT Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date, Rating, Admin_b, S_Blocked,Supporter_b FROM Worker, Account WHERE Email = Worker_Email;";
            dr = com.ExecuteReader();
            com.CommandType = System.Data.CommandType.Text;
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
                workersDetails.worker.Rating =float.Parse(dr["Rating"].ToString());
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
            return View(workers);
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
