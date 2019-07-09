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
            var customers = context.Customers
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.Accounts).ToList();                            

            return View("Index", customers);
            
        }
    }
}