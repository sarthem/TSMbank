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
                Individual = new Individual()
                {
                    Email = appUser.Email
                },
                ModificationAction = ModificationAction.NewIndividual,
            };
            return View("IndividualForm", modelView);
        }


        [HttpPost]
        public ActionResult Save(IndividualFormViewModel individualViewFormModel)
        {           
            var appUser = context.Users.Find(User.Identity.GetUserId());
            if (!ModelState.IsValid)
            {
                var viewModel = new IndividualFormViewModel()
                {
                    Individual = individualViewFormModel.Individual,
                    Phones = individualViewFormModel.Phones,
                    PrimaryAddress = individualViewFormModel.PrimaryAddress
                };
                return View("IndividualForm", viewModel);
            }

            if (individualViewFormModel.IndividualId == null)
            {
                var individual = individualViewFormModel.Individual;
                individual.Id = appUser.Id;
                appUser.RegisterCompletion = true;

                individual.Phones = individualViewFormModel.Phones;
                individual.PrimaryAddress = individualViewFormModel.PrimaryAddress;
                context.Individuals.Add(individual);

                var userId = User.Identity.GetUserId();
               

                var petition = new Request()
                {
                    IndividualId = userId,
                    SubmissionDate = DateTime.Now,
                    Individual = individual.User,
                    Status = RequestStatus.Pending,
                    Type = RequestType.UserAccActivation
                };
                context.Requests.Add(petition);
            }
            else
            {
                var intividualDB = context.Individuals.Include(c => c.Phones)
                                                    .Include(c => c.PrimaryAddress)
                                                    .Include(c => c.SecondaryAddress)                                                    
                                                    .SingleOrDefault(c => c.Id == individualViewFormModel.IndividualId);

                switch (individualViewFormModel.ModificationAction)
                {
                    case ModificationAction.EditIndividual:
                        intividualDB.DateOfBirth = individualViewFormModel.Individual.DateOfBirth;
                        intividualDB.Email = individualViewFormModel.Individual.Email;
                        intividualDB.FathersName = individualViewFormModel.Individual.FathersName;
                        intividualDB.FirstName = individualViewFormModel.Individual.FirstName;
                        intividualDB.IdentificationCardNo = individualViewFormModel.Individual.IdentificationCardNo;
                        intividualDB.LastName = individualViewFormModel.Individual.LastName;
                        intividualDB.SSN = individualViewFormModel.Individual.SSN;
                        intividualDB.VatNumber = individualViewFormModel.Individual.VatNumber;
                        break;

                    case ModificationAction.EditAddresses:
                        intividualDB.PrimaryAddress.City = individualViewFormModel.PrimaryAddress.City;
                        intividualDB.PrimaryAddress.Country = individualViewFormModel.PrimaryAddress.Country;
                        intividualDB.PrimaryAddress.PostalCode = individualViewFormModel.PrimaryAddress.PostalCode;
                        intividualDB.PrimaryAddress.Region = individualViewFormModel.PrimaryAddress.Region;
                        intividualDB.PrimaryAddress.Street = individualViewFormModel.PrimaryAddress.Street;
                        intividualDB.PrimaryAddress.StreetNumber = individualViewFormModel.PrimaryAddress.StreetNumber;
                        break;

                    case ModificationAction.EditPhones:
                        for (int i = 0; i < intividualDB.Phones.Count; i++)
                        {
                            intividualDB.Phones.ElementAt(i).CountryCode = individualViewFormModel.Phones[i].CountryCode;
                            intividualDB.Phones.ElementAt(i).PhoneNumber = individualViewFormModel.Phones[i].PhoneNumber;
                            intividualDB.Phones.ElementAt(i).PhoneType = individualViewFormModel.Phones[i].PhoneType;
                        }
                        break;
                    default:
                        break;
                }
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(string id, int modify)
        {
            var individual = context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.SecondaryAddress)
                            .SingleOrDefault(c => c.Id == id);

            if (individual == null)
                return HttpNotFound();

            var viewModel = new IndividualFormViewModel
            {
                Phones = individual.Phones.ToList(),
                PrimaryAddress = individual.PrimaryAddress,
                SecondaryAddress = individual.SecondaryAddress,
                Individual = individual,                
                IndividualId = individual.Id,
                ModificationAction = (ModificationAction)Enum.Parse(typeof(ModificationAction), modify.ToString())
            };
            return View("IndividualForm", viewModel);
        }

        public ActionResult Details(string id, string detail)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var individual = context.Individuals
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
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

        public ActionResult BankAccountPetition(byte Id)
        {
            var userId = User.Identity.GetUserId();
            var user = context.Individuals.SingleOrDefault(u => u.Id == userId);
            var accountType = context.BankAccountTypes.SingleOrDefault(a => a.Id == Id);

            var petition = context.BankAccRequests
                    .SingleOrDefault(r => r.BankAccTypeId == accountType.Id && r.Status == RequestStatus.Pending);
            if (petition == null)
            {
                var request = new BankAccRequest()
                {
                    IndividualId = user.Id,
                    SubmissionDate = DateTime.Now,
                    Individual = user.User,
                    Type = RequestType.BankAccActivation,
                    Status = RequestStatus.Pending,
                    BankAccTypeId = Id,
                    BankAccSummury = accountType.Summary
                };
                context.BankAccRequests.Add(request);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(petition);
            }
            
            
            
        }

    }
}