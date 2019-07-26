using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    public class RequestsController : Controller
    {

        private ApplicationDbContext context;

        public RequestsController()
        {
            context = new ApplicationDbContext();
        }

        public ActionResult Index(RequestStatus status)
        {
            var parentQuery = context.Requests.Where(r => r.Status == status);
            var requests = parentQuery.Include(r => r.Individual).ToList();
            parentQuery.OfType<BankAccRequest>().Include(r => r.BankAccType).Load();

            var viewModel = new RequestsViewModel
            {
                Requests = requests.ToList(),
                Status = status
            };

            return View(viewModel);
        }


        public async Task<ActionResult> Handle(int id, RequestStatus requestStatus)
        {
            var request = context.Requests.Include(r => r.Individual.BankAccounts).SingleOrDefault(r => r.Id == id);

            if (requestStatus == RequestStatus.Approved)
                await request.Approve();
            else
                await request.Reject();
            context.SaveChanges();
            return RedirectToAction("Index", new { status = RequestStatus.Pending });
        }

        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult CardReqDetails(int id)
        {
            var cardReq = context.CardRequests.Include(r => r.Individual).SingleOrDefault(r => r.Id == id);

            if (cardReq == null)
                return HttpNotFound();

            return View(cardReq);
        }

        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult BankAccReqDetails(int id)
        {
            var bankAccReq = context.BankAccRequests
                    .Include(r => r.Individual)
                    .Include(r => r.BankAccType)
                    .SingleOrDefault(r => r.Id == id);

            if (bankAccReq == null)
                return HttpNotFound();

            return View(bankAccReq);
        }

        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult UserAccReqDetails(int id)
        {
            var userAccReq = context.Requests
                    .Include(r => r.Individual.Phones)
                    .Include(r => r.Individual.PrimaryAddress)
                    .SingleOrDefault(r => r.Id == id);

            if (userAccReq == null)
                return HttpNotFound();

            return View(userAccReq);
        }
    }
}