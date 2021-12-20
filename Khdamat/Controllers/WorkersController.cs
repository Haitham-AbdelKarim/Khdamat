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
        public IActionResult Register([Bind("Natoinal_ID,Client_Email,First_Name,Last_Name,Country,City,Street,Phone,Gender,Birth_Date")] Worker worker)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("Email") == null)
                    return RedirectToAction("Register", "Accounts");
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Client (Natoinal_ID, Client_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date) values ('"
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
                    com.CommandText = "UPDATE ACCOUNT SET Worker_b = '1' WHERE Email = '"
                        + HttpContext.Session.GetString("Email") + "';";
                    com.ExecuteNonQuery();
                    HttpContext.Session.SetInt32("isWorker", 1);
                    HttpContext.Session.SetString("FirstName", worker.First_Name.ToString());
                }
                catch (Exception error)
                {

                    throw error;
                }
                con.Close();
            }
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
    }
}
