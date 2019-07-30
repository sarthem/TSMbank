using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.Persistance;
using TSMbank.Repositories;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    [Authorize]
    public class IndividualsController : Controller
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;

        public IndividualsController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var individual = unitOfWork.Individuals.GetIndividual(userId);
            return View("Index", individual);
        }

        // GET: 
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult GetIndividuals()
        {
            var individuals = unitOfWork.Individuals.GetIndividuals();//1
            return View(individuals);
        }

       // GET: Individuals/newIndividuals
        public ActionResult NewIndividual()
        {
            return View("Index");
        }


        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();
            var appUser = unitOfWork.Users.GetUser(userId);

            var modelView = new IndividualFormViewModel()
            {
                Individual = Individual.NewForView(),
                Phones = new List<Phone>(),
                ModificationAction = ModificationAction.NewIndividual,
            };
            modelView.Individual.SetEmail(appUser);
            return View("IndividualForm", modelView);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Save(IndividualFormViewModel ifvm)
        {
            var userId = User.Identity.GetUserId();
            var appUser = unitOfWork.Users.GetUser(userId);            

            if (!ModelState.IsValid)
            {
                var viewModel = new IndividualFormViewModel()
                {
                    Individual = ifvm.Individual,
                    Phones = ifvm.Phones,
                    PrimaryAddress = ifvm.PrimaryAddress
                };
                return View("IndividualForm", viewModel);
            }
            Collection<Phone> viewPhones = new Collection<Phone>(ifvm.Phones);

            if (ifvm.IndividualId == null)
            {
                var individual = Individual.New(ifvm, appUser, viewPhones);
                appUser.RegisterCompletion = true;
                unitOfWork.Individuals.AddIndividual(individual);//2                
                var request = new Request(individual, RequestType.UserAccActivation);
                unitOfWork.Requests.AddRequest(request);//3                
            }
            unitOfWork.Complete();
            if (User.IsInRole(RoleName.Administrator)) return RedirectToAction("GetIndividuals");
            return RedirectToAction("Index");
        }

        public ActionResult Update(IndividualFormViewModel individualVM)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new IndividualFormViewModel(individualVM);
                switch (individualVM.ModificationAction)
                {
                    case ModificationAction.EditIndividual:
                        return View("EditIndividual", viewModel);

                    case ModificationAction.EditAddresses:
                        return View("EditPhones", viewModel);

                    case ModificationAction.EditPhones:
                        return View("EditPhones", viewModel);

                }
                return View("IndividualForm", viewModel);
            }
           
            var individualDB = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(individualVM.IndividualId);//4

            switch (individualVM.ModificationAction)
            {
                case ModificationAction.EditIndividual:
                    individualDB.Edit(individualVM.Individual);
                    break;
                case ModificationAction.EditAddresses:
                    individualDB.PrimaryAddress.Edit(individualVM.PrimaryAddress);
                    if (individualDB.SecondaryAddress != null)
                    {
                        individualDB.SecondaryAddress.Edit(individualVM.SecondaryAddress);
                    }
                    break;
                case ModificationAction.EditPhones:
                    for (int i = 0; i < individualDB.Phones.Count; i++)
                    {
                        individualDB.Phones.ElementAt(i).Edit(individualVM.Phones[i]);
                    }
                    if (individualDB.Phones.Count < individualVM.Phones.Count)
                    {
                        for (int j = individualDB.Phones.Count; j < individualVM.Phones.Count; j++)
                        {
                            individualDB.Phones.Add(individualVM.Phones[j]);
                        }
                    }
                    break;
                default:
                    break;
            }
            unitOfWork.Complete();
            if (User.IsInRole(RoleName.Administrator)) return RedirectToAction("GetIndividuals");
            return RedirectToAction("Index");
        }


        //GET

        public ActionResult Edit(string id, ModificationAction modify)
        {            
            var individualDb = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(id);//5

            if (individualDb == null)
                return HttpNotFound();

            var viewModel = new IndividualFormViewModel(individualDb)
            {
                ModificationAction = modify
            };
            switch (modify)
            {
                case ModificationAction.EditIndividual:
                    return View("EditIndividual", viewModel);

                case ModificationAction.EditAddresses:
                    return View("EditAddress", viewModel);

                case ModificationAction.EditPhones:
                    return View("EditPhones", viewModel);
            }
            return View("IndividualForm", viewModel);
        }

        //GET
        public ActionResult Details(string id, string detail)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var individual = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(id);//6

            if (individual == null)
            {
                return HttpNotFound();
            }

            switch (detail)
            {
                case "PersonalInfo":
                    return View("Details", individual);
                case "AddressInfo":
                    return View("DetailsAddress", individual);
                case "PhoneInfo":
                    return View("DetailsPhone", individual);
                default:
                    return View(individual);
            }
        }

        public ActionResult ActivateAccount(string id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var individual = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(id);//7

            if (individual == null) return HttpNotFound();

            if (individual.Status == IndividualStatus.Active) individual.Deactivate();
            else individual.Activate();

            unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult BankAccountPetition(byte id)
        {
            var userId = User.Identity.GetUserId();
            var individual = unitOfWork.Individuals.GetJustIndividual(userId);//8            
            var activeBankAccReq = unitOfWork.BankAccountRequests.GetBankAccountRequestWithStatus(userId, id);//9    
            
            if (activeBankAccReq == null)
            {
                var bankAccRequest = new BankAccRequest(individual, RequestType.BankAccActivation, id);
                unitOfWork.BankAccountRequests.AddBankAccountRequest(bankAccRequest);//10                
                unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            else
                return View(activeBankAccReq);
        }

        public ActionResult AddSecondAddress()
        {
            var address = new Address();
            return View(address);
        }

        [HttpPost]
        public ActionResult AddSecondAddressSave(Address address)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new Address(address);
                return View("AddSecondAddress", viewModel);
            }

            var userId = User.Identity.GetUserId();
            var user = unitOfWork.Individuals.GetJustIndividual(userId);            
            user.SecondaryAddress = address;
            unitOfWork.Complete();
            return RedirectToAction("Index");
        }
    }
}
//1
//context.Individuals
//                .Include(c => c.Phones)
//                .Include(c => c.PrimaryAddress)
//                .Include(c => c.BankAccounts).ToList();

//2
//context.Individuals.Add(individual);

//3
//context.Requests.Add(request);

//4
//var individualDB = context.Individuals.Include(c => c.Phones)
//                                           .Include(c => c.PrimaryAddress)
//                                           .Include(c => c.SecondaryAddress)
//                                           .SingleOrDefault(c => c.Id == individualVM.IndividualId);

//5
//var individualDb = context.Individuals.Include(c => c.Phones).Include(c => c.PrimaryAddress)
//                .Include(c => c.SecondaryAddress)
//                .SingleOrDefault(c => c.Id == id);

//6
//context.Individuals
//            .Include(c => c.Phones)
//            .Include(c => c.PrimaryAddress)
//            .Include(c => c.SecondaryAddress)
//            .SingleOrDefault(c => c.Id == id);

//7
//context.Individuals
//            .Include(ph => ph.Phones)
//            .Include(a => a.PrimaryAddress)
//            .SingleOrDefault(c => c.Id == id);

//8
//context.Individuals.SingleOrDefault(u => u.Id == userId);

//9
//context.BankAccRequests
//                    .Include(r => r.BankAccType)
//                    .SingleOrDefault(r => r.IndividualId == userId
//                    && r.BankAccTypeId == id
//                    && r.Status == RequestStatus.Pending || r.Status == RequestStatus.Approved);

//10
//context.BankAccRequests.Add(bankAccReq);

//// Code only for testing/debugging. Fetch modelstate errors.
//var errors = new List<ModelState>();
//foreach (ModelState modelState in ModelState.Values)
//{
//    if (modelState.Errors.Count > 0)
//    {
//        errors.Add(modelState);
//    }
//}