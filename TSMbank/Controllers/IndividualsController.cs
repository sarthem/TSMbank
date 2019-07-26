using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;

namespace TSMbank.Controllers
{
    [Authorize]
    public class IndividualsController : Controller
    {
        private ApplicationDbContext context;        

        public IndividualsController()
        {
            context = new ApplicationDbContext();           
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        
        public ActionResult Index()
        {
            var appUser = context.Users.Find(User.Identity.GetUserId());

            var individual = context.Individuals
                                .Include(c => c.Phones)
                                .Include(c => c.PrimaryAddress)
                                .Include(c => c.BankAccounts)                            
                                .SingleOrDefault(c => c.Id == appUser.Id);

            return View("Index", individual);
        }

        // GET: 
        [Authorize(Roles = RoleName.Administrator)]
        public ActionResult GetIndividuals()
        {
            var individuals = context.Individuals
                                .Include(c => c.Phones)
                                .Include(c => c.PrimaryAddress)
                                .Include(c => c.BankAccounts).ToList();

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
            var appUser = context.Users.Find(userId);

            var modelView = new IndividualFormViewModel()
            {
                Individual = new Individual(),  
                Phones = new List<Phone>(),                
                ModificationAction = ModificationAction.NewIndividual,
            };
            modelView.Individual.SetEmail(appUser);

            return View("IndividualForm", modelView);
        }


        [HttpPost]
        [Authorize]
        public ActionResult Save(IndividualFormViewModel individualViewFormModel)
        {           
            var userId = User.Identity.GetUserId();
            var appUser = context.Users.Single(a => a.Id == userId);

            // Code only for testing/debugging. Fetch modelstate errors.
            var errors = new List<ModelState>();
            foreach (ModelState modelState in ModelState.Values)
            {
                if(modelState.Errors.Count > 0)
                {
                    errors.Add(modelState);
                }
            }   

            if (!ModelState.IsValid)
            {
                var viewModel = new IndividualFormViewModel(individualViewFormModel);
                
                return View("IndividualForm", viewModel);
            }

            if (individualViewFormModel.IndividualId == null)
            {
                var individual = new Individual(individualViewFormModel.Individual, appUser);
                individual.New(individualViewFormModel);
                appUser.RegisterCompletion = true;

                context.Individuals.Add(individual);
                var request = new Request(individual, RequestType.UserAccActivation);
                context.Requests.Add(request);
            }
            else
            {
                var individualDB = context.Individuals.Include(c => c.Phones)
                                                    .Include(c => c.PrimaryAddress)
                                                    .Include(c => c.SecondaryAddress)                                                    
                                                    .SingleOrDefault(c => c.Id == individualViewFormModel.IndividualId);

                switch (individualViewFormModel.ModificationAction)
                {
                    case ModificationAction.EditIndividual:
                        individualDB.Edit(individualViewFormModel.Individual);
                        break;

                    case ModificationAction.EditAddresses:
                        individualDB.PrimaryAddress.Edit(individualViewFormModel.PrimaryAddress);  
                        if (individualDB.SecondaryAddress != null)
                        {
                            individualDB.SecondaryAddress.Edit(individualViewFormModel.SecondaryAddress);
                        }
                        break;

                    case ModificationAction.EditPhones:
                        for (int i = 0; i < individualDB.Phones.Count; i++)
                        {                            
                            individualDB.Phones.ElementAt(i).Edit(individualViewFormModel.Phones[i]);
                        }
                        if(individualDB.Phones.Count < individualViewFormModel.Phones.Count)
                        {
                            for (int j = individualDB.Phones.Count; j < individualViewFormModel.Phones.Count; j++)
                            {
                                individualDB.Phones.Add(individualViewFormModel.Phones[j]);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            context.SaveChanges();
            if(User.IsInRole(RoleName.Administrator)) return RedirectToAction("GetIndividuals");
            return RedirectToAction("Index");
        }

        //GET
        public ActionResult Edit(string id, int modify)
        {
            var individual = context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.SecondaryAddress)
                            .SingleOrDefault(c => c.Id == id);

            if (individual == null)
                return HttpNotFound();

            var viewModel = new IndividualFormViewModel(individual)
            {
                ModificationAction = (ModificationAction)Enum.Parse(typeof(ModificationAction), modify.ToString())
            };
            return View("IndividualForm", viewModel);
        }

        //GET
        public ActionResult Details(string id, string detail)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var individual = context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.SecondaryAddress)
                            .SingleOrDefault(c => c.Id == id);

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

            var individual = context.Individuals
                            .Include(ph => ph.Phones)
                            .Include(a => a.PrimaryAddress)
                            .SingleOrDefault(c => c.Id == id);

            if (individual == null) return HttpNotFound();

            if (individual.Status ==  IndividualStatus.Active)
                individual.Status = IndividualStatus.Inactive;
            else individual.Status = IndividualStatus.Active;

            context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult BankAccountPetition(byte id)
        {
            var userId = User.Identity.GetUserId();
            var individual = context.Individuals.SingleOrDefault(u => u.Id == userId);
            //var accountType = context.BankAccountTypes.SingleOrDefault(a => a.Id == Id);

            // May cause problems (needs refactoring??)
            var activeBankAccReq = context.BankAccRequests
                                    .SingleOrDefault(r => r.BankAccTypeId == id && 
                                    r.Status == RequestStatus.Pending);

            if (activeBankAccReq == null)
            {
                var bankAccReq = new BankAccRequest(individual,RequestType.BankAccActivation, id);
                context.BankAccRequests.Add(bankAccReq);
                context.SaveChanges();
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
            context.SaveChanges();   

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