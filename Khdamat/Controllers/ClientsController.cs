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
    public class ClientsController : Controller
    {
        private readonly ApplicationDbContext _context;
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            return View(await _context.Client.ToListAsync());
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Natoinal_ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Register/
        public IActionResult Register()
        {
            if(HttpContext.Session.GetString("Email") == null)
                return RedirectToAction("Register", "Accounts");
            if (HttpContext.Session.GetInt32("isClient") != null)
                return RedirectToAction("Register", "Accounts");
            Client client = new Client();
            return View(client);
        }

        // POST: Clients/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Natoinal_ID,Client_Email,First_Name,Last_Name,Country,City,Street,Phone,Gender,Birth_Date")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("Email") == null)
                    return RedirectToAction("Register", "Accounts");
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Client (Natoinal_ID, Client_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date) values ('" 
                    + client.Natoinal_ID + "','" 
                    + HttpContext.Session.GetString("Email") + "','"
                    + client.First_Name + "','"
                    + client.Last_Name + "','"
                    + client.Country + "','"
                    + client.City + "','"
                    + client.Street + "','"
                    + client.Phone + "','"
                    + client.Gender.ToString()+ "','"
                    + client.Birth_Date.ToString("yyyy-MM-dd")
                    + "');";
                try
                {
                    com.ExecuteNonQuery();
                    com.CommandText = "UPDATE ACCOUNT SET Client_b = '1' WHERE Email = '"
                        + HttpContext.Session.GetString("Email") + "';";
                    com.ExecuteNonQuery();
                    HttpContext.Session.SetInt32("isClient",1);
                    HttpContext.Session.SetString("FirstName", client.First_Name.ToString());
                }
                catch (Exception error)
                {

                    throw error;
                }
                con.Close();
                return RedirectToAction("Index", "Home");
            }
            return View(client);
        }


        // GET: Clients/RegisterBoth
        public IActionResult RegisterBoth()
        {
            if (HttpContext.Session.GetString("Email") == null)
                return RedirectToAction("Register", "Accounts");
            if (HttpContext.Session.GetInt32("isClient") != null)
                return RedirectToAction("Register", "Accounts");
            Client client = new Client();
            return View(client);
        }

        // POST: Clients/RegisterBoth
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterBoth([Bind("Natoinal_ID,Client_Email,First_Name,Last_Name,Country,City,Street,Phone,Gender,Birth_Date")] Client client)
        {
            if (ModelState.IsValid)
            {
                if (HttpContext.Session.GetString("Email") == null)
                    return RedirectToAction("Register", "Accounts");
                con.Open();
                com.Connection = con;
                com.CommandText = "INSERT INTO Client (Natoinal_ID, Client_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date) values ('"
                    + client.Natoinal_ID + "','"
                    + HttpContext.Session.GetString("Email") + "','"
                    + client.First_Name + "','"
                    + client.Last_Name + "','"
                    + client.Country + "','"
                    + client.City + "','"
                    + client.Street + "','"
                    + client.Phone + "','"
                    + client.Gender.ToString() + "','"
                    + client.Birth_Date.ToString("yyyy-MM-dd")
                    + "');";
                try
                {
                    com.ExecuteNonQuery();
                    com.CommandText = "UPDATE ACCOUNT SET Client_b = '1' WHERE Email = '"
                        + HttpContext.Session.GetString("Email") + "';";
                    com.ExecuteNonQuery();
                    HttpContext.Session.SetInt32("isClient", 1);
                    HttpContext.Session.SetString("FirstName", client.First_Name.ToString());
                }
                catch (Exception error)
                {

                    throw error;
                }

                com.Connection = con;
                com.CommandText = "INSERT INTO Worker (Natoinal_ID, Worker_Email, F_Name, L_Name, Country, City, Street, Phone, Gender, Birth_Date) values ('"
                    + client.Natoinal_ID + "','"
                    + HttpContext.Session.GetString("Email") + "','"
                    + client.First_Name + "','"
                    + client.Last_Name + "','"
                    + client.Country + "','"
                    + client.City + "','"
                    + client.Street + "','"
                    + client.Phone + "','"
                    + client.Gender.ToString() + "','"
                    + client.Birth_Date.ToString("yyyy-MM-dd")
                    + "');";
                try
                {
                    com.ExecuteNonQuery();
                    com.CommandText = "UPDATE ACCOUNT SET Worker_b = '1' WHERE Email = '"
                        + HttpContext.Session.GetString("Email") + "';";
                    com.ExecuteNonQuery();
                    HttpContext.Session.SetInt32("isWorker", 1);
                }
                catch (Exception error)
                {

                    throw error;
                }

                con.Close();
                return RedirectToAction("Index", "Home");
            }
            return View(client);
        }


        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Natoinal_ID,Client_Email,First_Name,Last_Name,Country,City,Street,Phone,Gender,Birth_Date")] Client client)
        {
            if (id != client.Natoinal_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.Natoinal_ID))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _context.Client
                .FirstOrDefaultAsync(m => m.Natoinal_ID == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var client = await _context.Client.FindAsync(id);
            _context.Client.Remove(client);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(string id)
        {
            return _context.Client.Any(e => e.Natoinal_ID == id);
        }
    }
}
