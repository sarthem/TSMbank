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
            var individuals = unitOfWork.Individuals.GetIndividuals();

            //context.Individuals
            //                .Include(c => c.Phones)
            //                .Include(c => c.PrimaryAddress)
            //                .Include(c => c.BankAccounts).ToList();

            return View(individuals);
        }

        // GET: Individuals/newIndividuals
        public ActionResult NewIndividual()
        {
            return View("Index");
        }


        public ActionResult New()
        {
            var appUser = context.Users.Find(User.Identity.GetUserId());

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
        public ActionResult Save(IndividualFormViewModel IFVM)
        {
            var userId = User.Identity.GetUserId();
            var appUser = context.Users.Single(a => a.Id == userId);
            var viewInd = IFVM.Individual;
            var viewAdr = IFVM.PrimaryAddress;

            // Code only for testing/debugging. Fetch modelstate errors.
            var errors = new List<ModelState>();
            foreach (ModelState modelState in ModelState.Values)
            {
                if (modelState.Errors.Count > 0)
                {
                    errors.Add(modelState);
                }
            }
            if (!ModelState.IsValid)
            {
                var viewModel = new IndividualFormViewModel()
                {
                    Individual = IFVM.Individual,
                    Phones = IFVM.Phones,
                    PrimaryAddress = IFVM.PrimaryAddress
                };

                return View("IndividualForm", viewModel);
            }
            Collection<Phone> viewPhones = new Collection<Phone>(IFVM.Phones);



            if (IFVM.IndividualId == null)
            {
                var individual = Individual.New(IFVM, appUser, viewPhones);
                appUser.RegisterCompletion = true;
                context.Individuals.Add(individual);
                var request = new Request(individual, RequestType.UserAccActivation);
                context.Requests.Add(request);
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

            //var individualDB = context.Individuals.Include(c => c.Phones)
            //                                           .Include(c => c.PrimaryAddress)
            //                                           .Include(c => c.SecondaryAddress)
            //                                           .SingleOrDefault(c => c.Id == individualVM.IndividualId);
            var individualDB = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(individualVM.IndividualId);



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
            //var individualDb = context.Individuals.Include(c => c.Phones).Include(c => c.PrimaryAddress)
            //                .Include(c => c.SecondaryAddress)
            //                .SingleOrDefault(c => c.Id == id);
            var individualDb = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(id);

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

            var individual = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(id);

            //context.Individuals
            //            .Include(c => c.Phones)
            //            .Include(c => c.PrimaryAddress)
            //            .Include(c => c.SecondaryAddress)
            //            .SingleOrDefault(c => c.Id == id);


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

            var individual = unitOfWork.Individuals.GetIndividualWithAddressAndPhone(id);

            //context.Individuals
            //            .Include(ph => ph.Phones)
            //            .Include(a => a.PrimaryAddress)
            //            .SingleOrDefault(c => c.Id == id);


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
            var individual = context.Individuals.SingleOrDefault(u => u.Id == userId);

            var activeBankAccReq = context.BankAccRequests
                                    .Include(r => r.BankAccType)
                                    .SingleOrDefault(r => r.IndividualId == userId
                                    && r.BankAccTypeId == id
                                    && r.Status == RequestStatus.Pending || r.Status == RequestStatus.Approved);

            if (activeBankAccReq == null)
            {
                var bankAccReq = new BankAccRequest(individual, RequestType.BankAccActivation, id);
                context.BankAccRequests.Add(bankAccReq);
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
            var user = context.Individuals
                        .SingleOrDefault(u => u.Id == userId);

            user.SecondaryAddress = address;
            unitOfWork.Complete();

            return RedirectToAction("Index");
        }
    }
}

