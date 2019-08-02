using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;
using System.Data.Entity;
using TSMbank.Hubs;

namespace TSMbank.Controllers
{
    public class CardsController : Controller
    {
        private ApplicationDbContext context;

        public CardsController()
        {
            context = new ApplicationDbContext();
        }

        // GET: Cards
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CreateCardReq(CardType cardType)
        {
            var userId = User.Identity.GetUserId();
            var individual = context.Individuals.SingleOrDefault(i => i.Id == userId);

            switch (cardType)
            {
                case CardType.CreditCard:
                    var viewModel = new CardReqViewModel()
                    {
                        IndividualStatus = individual.Status,
                        CardType = CardType.CreditCard
                    };
                    return View("CreditCard", viewModel);
                case CardType.PrepaidCard:
                    return RedirectToAction("Index", "Individuals");
                default:
                    return RedirectToAction("Index", "Individuals");
            }
        }

        [HttpPost]
        public ActionResult CreateCardReq(CardReqViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("CreditCard", viewModel);

            var userId = User.Identity.GetUserId();
            var individual = context.Individuals.SingleOrDefault(i => i.Id == userId);

            var creditCardReq = new CardRequest(individual, RequestType.CardActivation, viewModel.CreditLimit.Value,
                viewModel.TransactionAmountLimit.Value, CardType.CreditCard);

            context.CardRequests.Add(creditCardReq);
            context.SaveChanges();

            var hubModel = new { Name = individual.FullName, Type = CardType.CreditCard.ToString() };
            SignalHub.GetRequest(hubModel);

            return RedirectToAction("Index", "Individuals");

        }


        public ActionResult CardDetails(string id)
        {
            var card = context.Cards
                    .Include(c => c.BankAccount.Individual.PrimaryAddress)
                    .Include(c => c.BankAccount.BankAccountType)
                    .SingleOrDefault(c => c.Id == id);

            switch (card.Type)
            {
                case CardType.DebitCard:
                    return RedirectToAction("Index", "Individuals");
                case CardType.CreditCard:
                    return View("CreditCardDetails", card);
                case CardType.PrepaidCard:
                    return RedirectToAction("Index", "Individuals");
                default:
                    return RedirectToAction("Index", "Individuals");
            }
        }
    }
}