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
            var requests = context.Requests
                .Include(r => r.Individual)
                .Where(r => r.Status == status)
                .ToList();

            var viewModel = new RequestsViewModel
            {
                Requests = requests,
                Status = status
            };

            return View(viewModel);
        }


        public async Task<ActionResult> Handle(int id, RequestStatus requestStatus)
        {
            var request = context.Requests.Include(r => r.Individual).SingleOrDefault(r => r.Id == id);
            switch (request.Type)
            {
                case RequestType.UserAccActivation:
                    if (requestStatus == RequestStatus.Approved)
                        await request.Approve();
                    else if (requestStatus == RequestStatus.Rejected)
                        await request.Reject();

                    context.SaveChanges();
                    return RedirectToAction("Index", new { status = RequestStatus.Pending });

                case RequestType.BankAccActivation:
                    if (requestStatus == RequestStatus.Approved)
                    {
                        request.Status = RequestStatus.Processing;
                        context.SaveChanges();
                        return RedirectToAction("NewAccountRequest", "BankAccounts", new { requestId = request.Id });
                    }

                    await ((BankAccRequest)request).Reject();
                    context.SaveChanges();
                    return RedirectToAction("Index", new { status = RequestStatus.Pending });

                case RequestType.CardActivation:
                    break;
                default:
                    break;
            }
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
    }
}