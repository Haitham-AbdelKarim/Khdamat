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
    public class AccountsController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
        SqlDataReader dr;
        //private readonly ApplicationDbContext _context;

        public AccountsController(/*ApplicationDbContext context*/)
        {
            //_context = context;
            con.ConnectionString = Khdamat.Properties.Resources.ConnectionString;
        }

        // GET: Accounts
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Account.ToListAsync());
        //}

        // GET: Accounts/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    //var account = await _context.Account
        //    //    .FirstOrDefaultAsync(m => m.Email == id);
        //    if (account == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(account);
        //}

        // GET: Accounts/Register
        public IActionResult Register()
        {
            Account account = new Account();
            return View(account);
        }

        // POST: Accounts/Register
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register([Bind("Email,Password,ConfirmPassword,IsBlocked,IsAdmin,IsSupporter,IsWorker,IsClient")] Account account)
        {
            if (ModelState.IsValid)
            {
                    con.Open();
                    com.Connection = con;
                    com.CommandText = "INSERT INTO Account (Email, Password, S_Blocked) values ('" + account.Email.ToString() + "','" + account.Password.ToString() + "', '0');";
                    com.ExecuteNonQuery();
                    con.Close();
                    HttpContext.Session.SetString("Email", account.Email);
                if (account.IsClient && account.IsWorker)
                    return RedirectToAction(actionName: "RegisterBoth", controllerName: "Clients");
                else if (account.IsClient)
                    return RedirectToAction(actionName: "Register", controllerName: "Clients");
                else if(account.IsWorker)
                    return RedirectToAction(actionName: "Register", controllerName: "Workers");

            }
            return View(account);
        }

        // GET: Accounts/Login
        public IActionResult Login()
        {
            Account account = new Account();
            return View(account);
        }

        // POST: Accounts/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Email,Password")] Account account)
        {
            con.Open();
            com.Connection = con;
            com.CommandText = "SELECT * FROM Account WHERE Email = '" + account.Email+ "';";
            dr = com.ExecuteReader();
            if(dr == null)
            {
                return View(account);
            }
            dr.Read();
            HttpContext.Session.SetString("Email", dr["Email"].ToString());
            if (dr["Admin_b"] != null && dr["Admin_b"].ToString() == "True")
            {
                HttpContext.Session.SetInt32("isAdmin", 1);
            }
            if (dr["Supporter_b"] != null && dr["Supporter_b"].ToString() == "True")
            {
                HttpContext.Session.SetInt32("isSupporter", 1);
            }
            if (dr["Worker_b"] != null && dr["Worker_b"].ToString() == "True")
            {
                HttpContext.Session.SetInt32("isWorker", 1);
            }
            string s = dr["Client_b"].ToString();
            if (dr["Client_b"] != null && dr["Client_b"].ToString() == "True")
            {
                HttpContext.Session.SetInt32("isClient", 1);
            }
            if (dr["S_Blocked"] != null && dr["S_Blocked"].ToString() == "1")
                HttpContext.Session.SetInt32("isBlocked", 1);


            dr.Close();

            if (HttpContext.Session.GetInt32("isAdmin") == 1)
            {
                com.CommandText = "SELECT F_Name FROM Administrator WHERE Admin_Email = '" + account.Email + "';";
                dr = com.ExecuteReader();
                dr.Read();
                HttpContext.Session.SetString("FirstName", dr["F_Name"].ToString());
            }
            if (HttpContext.Session.GetInt32("isSupporter") == 1)
            {
                if (HttpContext.Session.GetString("FirstName") == null)
                {
                    com.CommandText = "SELECT F_Name FROM Supporter WHERE Supporter_Email = '" + account.Email + "';";
                    dr = com.ExecuteReader();
                    dr.Read();
                    HttpContext.Session.SetString("FirstName", dr["F_Name"].ToString());
                }
            }
            if (HttpContext.Session.GetInt32("isWorker") == 1)
            {
                if (HttpContext.Session.GetString("FirstName") == null)
                {
                    com.CommandText = "SELECT F_Name FROM Worker WHERE Worker_Email = '" + account.Email + "';";
                    dr = com.ExecuteReader();
                    dr.Read();
                    HttpContext.Session.SetString("FirstName", dr["F_Name"].ToString());
                }
            }
            if (HttpContext.Session.GetInt32("isClient") == 1)
            {
                if (HttpContext.Session.GetString("FirstName") == null)
                {
                    com.CommandText = "SELECT F_Name FROM Client WHERE Client_Email = '" + account.Email + "';";
                    dr = com.ExecuteReader();
                    dr.Read();
                    HttpContext.Session.SetString("FirstName", dr["F_Name"].ToString());
                }
            }
            dr.Close();
            con.Close();
            if (HttpContext.Session.GetString("FirstName") == null)
                return RedirectToAction(actionName: "Choose", controllerName: "Accounts");

            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        // GET: Accounts/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(actionName: "Index", controllerName: "Home");
        }

        // GET: Accounts/Choose
        public IActionResult Choose()
        {
            if(HttpContext.Session.GetString("FirstName") != null)
                return RedirectToAction(actionName: "Index", controllerName: "Home");
            Account account = new Account();
            return View(account);
        }

        // POST: Accounts/Choose
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Choose([Bind("IsWorker,IsClient")] Account account)
        {
            if(account.IsClient && account.IsWorker)
                return RedirectToAction(actionName: "RegisterBoth", controllerName: "Clients");
            else if (account.IsClient)
                return RedirectToAction(actionName: "Register", controllerName: "Clients");
            else if (account.IsWorker)
                return RedirectToAction(actionName: "Register", controllerName: "Workers");
            else
                return View(account);
        }


        // GET: Accounts/Edit/5
        //    public async Task<IActionResult> Edit(string id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var account = await _context.Account.FindAsync(id);
        //        if (account == null)
        //        {
        //            return NotFound();
        //        }
        //        return View(account);
        //    }

        //    // POST: Accounts/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to.
        //    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(string id, [Bind("Email,Password,ConfirmPassword,IsBlocked,IsAdmin,IsSupporter,IsWorker,IsClient")] Account account)
        //    {
        //        if (id != account.Email)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(account);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!AccountExists(account.Email))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        return View(account);
        //    }

        //    // GET: Accounts/Delete/5
        //    public async Task<IActionResult> Delete(string id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var account = await _context.Account
        //            .FirstOrDefaultAsync(m => m.Email == id);
        //        if (account == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(account);
        //    }

        //    // POST: Accounts/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(string id)
        //    {
        //        var account = await _context.Account.FindAsync(id);
        //        _context.Account.Remove(account);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool AccountExists(string id)
        //    {
        //        return _context.Account.Any(e => e.Email == id);
        //    }
    }
}
