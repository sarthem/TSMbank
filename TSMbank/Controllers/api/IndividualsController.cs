using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Models;

namespace TSMbank.Controllers.api
{
    public class IndividualsController : ApiController
    {
        private ApplicationDbContext context;

        public IHttpActionResult ChangeCustomerStatus(string id)
        {
            var individual = context.Individuals.SingleOrDefault(c => c.Id == id);

            if (individual == null)
                return NotFound();

            individual.Status = individual.Status == IndividualStatus.Active ? IndividualStatus.Inactive : IndividualStatus.Active;
            context.SaveChanges();
            return Ok();
        }


        public IndividualsController()
        {
            context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}
