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
using TSMbank.Persistance;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    public class RequestsController : Controller
    {

        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;

        public RequestsController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }

        public ActionResult Index(RequestStatus status)
        {
            var parentQuery = unitOfWork.Requests.GetRequestsByStatus(status);//1
              
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
            var request = unitOfWork.Requests.GetRequest(id);//2               

            if (requestStatus == RequestStatus.Approved)
                await request.Approve();
            else
                await request.Reject();
            unitOfWork.Complete();//3            
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
            var bankAccRequest = unitOfWork.BankAccountRequests.GetBankAccRequest(id);//4

            if (bankAccRequest == null)
                return HttpNotFound();

            return View(bankAccRequest);
        }

        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult UserAccReqDetails(int id)
        {
            var userAccountRequest = unitOfWork.Requests.GetUserAccRequest(id);//5            

            if (userAccountRequest == null)
                return HttpNotFound();

            return View(userAccountRequest);
        }
    }
}

