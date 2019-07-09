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
    public class CustomersController : Controller
    {
        private ApplicationDbContext context;        

        public CustomersController()
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

            var customer = context.Customers
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.Accounts)
                            .SingleOrDefault(c => c.ApplicationUserId == appUser.Id);

            return View("Index", customer);
        }
           

        // GET: Customers/newcustomers
        public ActionResult newCustomer()
        {
            var customer = context.Customers
                .Include(c => c.Phones)
                .Include(c => c.PrimaryAddress)
                .Include(c => c.Accounts)
                .SingleOrDefault(c => c.Id == 0);
                
            return View("Index", customer);
        }


        public ActionResult New()
        {
            var userId = User.Identity.GetUserId();            
            var appUser = context.Users.Find(userId);
            var modelView = new CustomerFormViewModel()
            {
                Customer = new Customer()
                {
                    Email = appUser.Email
                },
                ModificationAction = ModificationAction.NewCustomer,
            };
            return View("CustomerForm", modelView);
        }


        [HttpPost]
        public ActionResult Save(CustomerFormViewModel customerViewFormModel)
        {           
            var appUser = context.Users.Find(User.Identity.GetUserId());
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel()
                {
                    Customer = customerViewFormModel.Customer,
                    Phones = customerViewFormModel.Phones,
                    PrimaryAddress = customerViewFormModel.PrimaryAddress
                };
                return View("CustomerForm", viewModel);
            }

            if (customerViewFormModel.CustomerId == 0)
            {
                var customer = customerViewFormModel.Customer;
                customer.ApplicationUserId = appUser.Id;
                appUser.RegisterCompletion = true;
                
                customer.Phones = customerViewFormModel.Phones;
                customer.PrimaryAddress = customerViewFormModel.PrimaryAddress;
                context.Customers.Add(customer);
            }
            else
            {
                var customerDB = context.Customers.Include(c => c.Phones)
                                                    .Include(c => c.PrimaryAddress)
                                                    .Include(c => c.SecondaryAddress)
                                                    .Include(c => c.Phones)//???
                                                    .SingleOrDefault(c => c.Id == customerViewFormModel.CustomerId);

                switch (customerViewFormModel.ModificationAction)
                {
                    case ModificationAction.EditCustomer:
                        customerDB.DateOfBirth = customerViewFormModel.Customer.DateOfBirth;
                        customerDB.Email = customerViewFormModel.Customer.Email;//?????
                        customerDB.FathersName = customerViewFormModel.Customer.FathersName;
                        customerDB.FirstName = customerViewFormModel.Customer.FirstName;
                        customerDB.IdentificationCardNo = customerViewFormModel.Customer.IdentificationCardNo;
                        customerDB.LastName = customerViewFormModel.Customer.LastName;
                        customerDB.SSN = customerViewFormModel.Customer.SSN;
                        customerDB.VatNumber = customerViewFormModel.Customer.VatNumber;
                        break;

                    case ModificationAction.EditAddresses:
                        customerDB.PrimaryAddress.City = customerViewFormModel.PrimaryAddress.City;
                        customerDB.PrimaryAddress.Country = customerViewFormModel.PrimaryAddress.Country;
                        customerDB.PrimaryAddress.PostalCode = customerViewFormModel.PrimaryAddress.PostalCode;
                        customerDB.PrimaryAddress.Region = customerViewFormModel.PrimaryAddress.Region;
                        customerDB.PrimaryAddress.Street = customerViewFormModel.PrimaryAddress.Street;
                        customerDB.PrimaryAddress.StreetNumber = customerViewFormModel.PrimaryAddress.StreetNumber;
                        break;

                    case ModificationAction.EditPhones:
                        for (int i = 0; i < customerDB.Phones.Count; i++)
                        {
                            customerDB.Phones.ElementAt(i).CountryCode = customerViewFormModel.Phones[i].CountryCode;
                            customerDB.Phones.ElementAt(i).PhoneNumber = customerViewFormModel.Phones[i].PhoneNumber;
                            customerDB.Phones.ElementAt(i).PhoneType = customerViewFormModel.Phones[i].PhoneType;
                        }
                        break;
                    default:
                        break;
                }
            }
            context.SaveChanges();
            return RedirectToAction("Index");
        }


        public ActionResult Edit(int? id, int modify)
        {
            var customer = context.Customers
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .Include(c => c.SecondaryAddress)
                            .SingleOrDefault(c => c.Id == id);

            if (customer == null)
                return HttpNotFound();

            var viewModel = new CustomerFormViewModel
            {
                Phones = customer.Phones.ToList(),
                PrimaryAddress = customer.PrimaryAddress,
                SecondaryAddress = customer.SecondaryAddress,
                Customer = customer,
                CustomerId = customer.Id,
                ModificationAction = (ModificationAction)Enum.Parse(typeof(ModificationAction), modify.ToString())
            };
            return View("CustomerForm", viewModel);
        }

        public ActionResult Details(int? id, string detail)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var customer = context.Customers
                            .Include(c => c.Phones)
                            .Include(c => c.PrimaryAddress)
                            .SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            switch (detail)
            {
                case "PersonalInfo":
                    return View("Details", customer);                    
                case "AddressInfo":
                    return View("DetailsAddress", customer);                    
                case "PhoneInfo":
                    return View("DetailsPhone", customer);
                default:
                    return View(customer);
            }
        }

            





        public ActionResult ActivateAccount(int? id)

        {
            if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = context.Customers
                            .Include(ph => ph.Phones)
                            .Include(a => a.PrimaryAddress)
                            .SingleOrDefault(c => c.Id == id);

            if (customer == null) return HttpNotFound();

            if (customer.Status == CustomerStatus.Active)
                customer.Status = CustomerStatus.Inactive;
            else customer.Status = CustomerStatus.Active;

            context.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}