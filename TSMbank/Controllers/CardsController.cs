﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TSMbank.Models;
using TSMbank.ViewModels;
using System.Data.Entity;

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

            var creditCardReq = new CardRequest(individual, RequestType.CardActivation, viewModel.CreditLimit,
                viewModel.TransactionAmountLimit, CardType.CreditCard);

            context.CardRequests.Add(creditCardReq);
            context.SaveChanges();
            return RedirectToAction("Index", "Individuals");

        }

        // GET
        public ActionResult New()
        {
            
        }
    }
}