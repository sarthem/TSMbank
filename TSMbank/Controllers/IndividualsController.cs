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
using TSMbank.Hubs;
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

            var viewModel = new IndividualFormViewModel()
            {
                Individual = Individual.NewForView(),
                Phones = new List<Phone>(),
                ModificationAction = ModificationAction.NewIndividual,
            };
            viewModel.Individual.SetEmail(appUser);
            return View("IndividualForm", viewModel);
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

                var hubModel = new { Name = ifvm.Individual.FullName, Type = request.Type.ToString() };
                SignalHub.GetRequest(hubModel);
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

                var hubModel = new { Name = individual.FullName , Type = bankAccRequest.Type.ToString() };
                SignalHub.GetAccRequest(hubModel);

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

        public ActionResult ViewCrypto()
        {
            return View();
        }

        public ActionResult ViewForex()
        {
            return View();
        }

        public ActionResult ViewHeatMap()
        {
            return View();
        }
    }
}
