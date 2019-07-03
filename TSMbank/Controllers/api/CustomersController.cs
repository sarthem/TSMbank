using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Models;

namespace TSMbank.Controllers.api
{
    public class CustomersController : ApiController
    {
        private ApplicationDbContext context;

        public IHttpActionResult ChangeCustomerStatus(int id)
        {
            var customer = context.Customers.SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return NotFound();

            customer.Status = customer.Status == CustomerStatus.Active ? CustomerStatus.Inactive : CustomerStatus.Active;
            context.SaveChanges();
            return Ok();
        }


        public CustomersController()
        {
            context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}
