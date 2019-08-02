using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Net.Mail;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using TSMbank.Persistance;

namespace TSMbank.Controllers
{

    public class BankAccountsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;

        public BankAccountsController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }

        // GET: Accounts
        public ActionResult Index()
        {
            return View();
        }
        
        // GET
        public ActionResult New(string individualId)
        {
            if (individualId == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var individual = unitOfWork.Individuals.GetJustIndividual(individualId);//1
               
            if (individual == null)
                return HttpNotFound();

            var bankAccount = new BankAccount();
            bankAccount.IndividualId = individual.Id;
            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = bankAccount,
                IndividualFullName = individual.FullName,
                BankAccountTypes = unitOfWork.BankAccountTypes.GetBankAccountTypes().ToList(),//2                
            };

            return View("BankAccountForm", viewModel);
        }

        [Authorize]//iparxei pia?
        public ActionResult NewAccountRequest(int requestId)
        {
            var bankAccReq = context.BankAccRequests
                                .Include(r => r.Individual)
                                .SingleOrDefault(r => r.Id == requestId);

            var viewModel = new BankAccountFormViewModel()
            {
                IndividualFullName = bankAccReq.Individual.FullName,
                BankAccountTypes = context.BankAccountTypes.Where(t => t.Id == bankAccReq.BankAccTypeId).ToList(),
                BankAccount = new BankAccount()
                {
                    IndividualId = bankAccReq.IndividualId
                }
            };

            return View("BankAccountForm", viewModel);
        }

        // GET 
        public ActionResult Edit(string accountNumber)
        {
            if (accountNumber == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bankAccount = context.BankAccounts
                                    .Include(a => a.Individual)
                                    .SingleOrDefault(a => a.AccountNumber == accountNumber);

            if (bankAccount == null)
                return HttpNotFound();

            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = bankAccount,
                BankAccountTypes = context.BankAccountTypes.ToList(),
                IndividualFullName = bankAccount.Individual.FullName
            };

            return View("EditNickName", viewModel);
        }

        //GET
        public ActionResult Details(string accountNumber)
        {
            if (accountNumber == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bankAccount = unitOfWork.BankAccounts.GetBankAccount(accountNumber);//3
            var acctype = unitOfWork.BankAccountTypes.GetBankAccountType(bankAccount.BankAccountTypeId);//4

            if (bankAccount == null)
                return HttpNotFound();

            var viewModel = new BankAccountFormViewModel()
            {
                BankAccount = bankAccount,
                BankAccountTypes = unitOfWork.BankAccountTypes.GetBankAccountTypes().ToList(),//5                
                IndividualFullName = bankAccount.Individual.FullName,
                AccoutTypeDescription = acctype.Summary
            };

            return View("Details", viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult> Save(BankAccount bankAccount)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new BankAccountFormViewModel()
                {
                    BankAccount = bankAccount,
                    IndividualFullName = unitOfWork.Individuals.GetJustIndividual(bankAccount.IndividualId).FullName,//6                    
                    BankAccountTypes = unitOfWork.BankAccountTypes.GetBankAccountTypes().ToList()//7
                };
                return View("BankAccountForm", viewModel);
            }

            if (bankAccount.AccountNumber == null)
            {
                bankAccount.AccountNumber = BankAccount.CreateRandomAccountNumber();
                bankAccount.OpenedDate = DateTime.Now;
                bankAccount.StatusUpdatedDateTime = DateTime.Now;
                unitOfWork.BankAccounts.AddBankAccount(bankAccount);//8

                var request = unitOfWork.BankAccountRequests
                                .GetBankAccRequestByStatus(bankAccount.IndividualId, RequestStatus.Processing);//9       
                await request.Approve();
            }
            else
            {
                var bankAccountInDb = unitOfWork.BankAccounts.GetBankAccount(bankAccount.AccountNumber);//10                    

                bankAccountInDb.WithdrawalLimit = bankAccount.WithdrawalLimit;
                bankAccountInDb.NickName = bankAccount.NickName;
            }
            unitOfWork.Complete();//11
            return RedirectToAction("index", "Individuals");
        }

        [Route("Accounts/ChangeStatus/{individualId}")]
        public ActionResult ChangeStatus(string accountNumber)
        {
            if (accountNumber == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var bankAccount = unitOfWork.BankAccounts.GetBankAccount(accountNumber);//12

            if (bankAccount == null)
                return HttpNotFound();

            bankAccount.AccountStatus = bankAccount.AccountStatus == AccountStatus.Active ? AccountStatus.Inactive : AccountStatus.Active;
            unitOfWork.Complete();//13
            return RedirectToAction("Index");
        }

        [Authorize(Roles = RoleName.Customer)]
        public ActionResult CheckingAccount()
        {
            var userId = User.Identity.GetUserId();
            var individual = unitOfWork.Individuals.GetJustIndividual(userId);//14
            var viewModel = new CheckingAccApplicationViewModel()
            {
                IndividualStatus = individual.Status,
            };
            return View(viewModel);
        }
       
        public ActionResult BankAccountSelection(Description description)
        {
            var userId = User.Identity.GetUserId();
            var individual = unitOfWork.Individuals.GetJustIndividual(userId);//15
            var viewModel = new CheckingAccApplicationViewModel() { IndividualStatus = individual.Status };
            switch (description)
            {
                case Description.Checking:                    
                    return View("CheckingAccount", viewModel);
                case Description.Savings:
                    return View("SavingAccount", viewModel);
                case Description.Term:
                    return View("SavingAccount", viewModel);
                case Description.CreditCard:
                    return View("SavingAccount", viewModel);
                default:
                    return View("Index");
            }
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}





