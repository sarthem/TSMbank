using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;

namespace TSMbank.Controllers
{
    public class AdminsController : Controller
    {

        private ApplicationDbContext context;

        public AdminsController()
        {
            context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        // GET: Admins
        public ActionResult Index()
        {
            var individuals = context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.BankAccounts).ToList();                            

            return View("Index", individuals);
            
        }
    }
}