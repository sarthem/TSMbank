using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Models;
using TSMbank.Persistance;

namespace TSMbank.Controllers.api
{
    public class IndividualsController : ApiController
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;

        public IndividualsController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }

        public IHttpActionResult ChangeCustomerStatus(string id)
        {
            var individual = unitOfWork.Individuals.GetJustIndividual(id);//1

            if (individual == null)
                return NotFound();

            if (individual.Status == IndividualStatus.Active) individual.Deactivate();
            else individual.Activate();

            unitOfWork.Complete();//2            
            return Ok();
        }
                
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}


//1
//context.Individuals.SingleOrDefault(c => c.Id == id);
//2
//context.SaveChanges();