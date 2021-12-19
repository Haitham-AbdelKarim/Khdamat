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

namespace Khdamat.Controllers
{
    public class AccountsController : Controller
    {
        SqlCommand com = new SqlCommand();
        SqlConnection con = new SqlConnection();
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
                    return RedirectToAction(actionName: "Register", controllerName: "Clients", new {account.Email});
            }
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
