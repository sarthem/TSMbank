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


        // GET: Customers
        public ActionResult Index()
        {
            var customers = context.Customers.Include(ph => ph.Phone).Include(ad => ad.Address).Include(ac => ac.Accounts).ToList();

            return View(customers);
        }

        public ActionResult New()
        {
            var customer = new CustomerViewFormModel()
            {
                Customer = new Customer(),
                CustomerId = 0,
                ModificationAction = ModificationAction.NewCustomer
            };
           
           

            return View(customer);
        }

        public ActionResult Save(CustomerViewFormModel customerViewFormModel)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = new CustomerViewFormModel()
                {
                    Customer = customerViewFormModel.Customer,
                    Phone = customerViewFormModel.Phone,
                    Address = customerViewFormModel.Address
                };
                return View("New", viewModel);
            }

            if (customerViewFormModel.CustomerId == 0)
            {
                context.Customers.Add(customerViewFormModel.Customer);
                context.Phones.Add(customerViewFormModel.Phone);
                context.Addresses.Add(customerViewFormModel.Address);
            }
            else
            {
                var customerDB = context.Customers.Include(ph => ph.Phone)
                    .Include(adr => adr.Address)
                    .SingleOrDefault(c => c.Id == customerViewFormModel.CustomerId);

                switch (customerViewFormModel.ModificationAction)
                {
                    case ModificationAction.EditCustomer:
                        customerDB.DateOfBirth = customerViewFormModel.Customer.DateOfBirth;
                        customerDB.Email = customerViewFormModel.Customer.Email;
                        customerDB.FatherName = customerViewFormModel.Customer.FatherName;
                        customerDB.FirstName = customerViewFormModel.Customer.FirstName;
                        customerDB.IdentificationCardNo = customerViewFormModel.Customer.IdentificationCardNo;
                        customerDB.LastName = customerViewFormModel.Customer.LastName;
                        customerDB.SSN = customerViewFormModel.Customer.SSN;
                        customerDB.VatNumber = customerViewFormModel.Customer.VatNumber;
                        break;

                    case ModificationAction.EditAddress:
                        customerDB.Address.City = customerViewFormModel.Address.City;
                        customerDB.Address.Country = customerViewFormModel.Address.Country;
                        customerDB.Address.PostalCode = customerViewFormModel.Address.PostalCode;
                        customerDB.Address.Region = customerViewFormModel.Address.Region;
                        customerDB.Address.Street = customerViewFormModel.Address.Street;
                        customerDB.Address.StreetNumber = customerViewFormModel.Address.StreetNumber;
                        break;

                    case ModificationAction.EditPhone:
                        customerDB.Phone.CountryCode = customerViewFormModel.Phone.CountryCode;
                        customerDB.Phone.PhoneNumber = customerViewFormModel.Phone.PhoneNumber;
                        customerDB.Phone.PhoneType = customerViewFormModel.Phone.PhoneType;
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
            var customer = context.Customers.Include(ph => ph.Phone).Include(ad => ad.Address).SingleOrDefault(c => c.Id == id);

            if (customer == null) return HttpNotFound();

            var viewModel = new CustomerViewFormModel
            {
                Phone = customer.Phone,
                Address = customer.Address,
                Customer = customer,
                CustomerId = customer.Id,
                ModificationAction = (ModificationAction)Enum.Parse(typeof(ModificationAction), modify.ToString())
            };
            return View("New",viewModel);
        }

        public ActionResult Details(int? id)
        {
            if (!id.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }


            var customer = context.Customers.Include(ph => ph.Phone).Include(a => a.Address)
                .SingleOrDefault(c => c.Id == id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }

        public ActionResult ActivateAccount(int? id)
        {
            if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);          

            var customer = context.Customers.Include(ph => ph.Phone).Include(a => a.Address)
                .SingleOrDefault(c => c.Id == id);

            if (customer == null) return HttpNotFound();            

            if (customer.IsActive == Activation.IsActive)
            {
                customer.IsActive = Activation.NotActive;
            }
            else
            {
                customer.IsActive = Activation.IsActive;
            }
                        
            context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult NewAccount(int? id)
        {
            if (!id.HasValue) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var customer = context.Customers.SingleOrDefault(c => c.Id == id);
            if (customer == null) return HttpNotFound();
            var bankAccount = new Account();
            var viewModel = new AccountViewFormForCustomerModel()
            {
                Customer = customer,
                Account = bankAccount
            };
            context.Accounts.Add(bankAccount);
            return View(viewModel);
        }
    }
}