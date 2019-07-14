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
    public class RequestsController : Controller
    {

        private ApplicationDbContext context;

        public RequestsController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Requests
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult GetRequests()
        {
            var requests = context.Requests
                .Include(r => r.Individual)
                .Include(i => i.Individual.Individual)
                .ToList();

            return View(requests);
        }

        public ActionResult RequestAnswer(int Id, RequestStatus requestStatus)
        {
            var requestDB = context.Requests.Include(r => r.Individual).SingleOrDefault(r => r.Id == Id);
            requestDB.Status = requestStatus;

            if (requestStatus == RequestStatus.Approved)
            {
                switch (requestDB.Type)
                {
                    case RequestType.UserAccActivation:
                        requestDB.Individual.Individual.Status = IndividualStatus.Active;
                        break;
                    case RequestType.BankAccActivation:
                       // nameof ere8i edw kai na kanei ton logariasmo
                        break;
                        //methodos pou stelnei email
            }

            }
           
            


            return RedirectToAction("GetRequests");
        }
        

    }
}