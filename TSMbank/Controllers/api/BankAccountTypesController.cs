using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Models;
using TSMbank.Dtos;

namespace TSMbank.Controllers.api
{
    public class BankAccountTypesController : ApiController
    {
        private ApplicationDbContext context;

        public BankAccountTypesController()
        {
            context = new ApplicationDbContext();
        }

        public IHttpActionResult GetAccountTypes()
        {
            var bankAccountTypes = context.BankAccountTypes.ToList().Select(Mapper.Map<BankAccountType, BankAccountTypeDto>);
            return Ok(bankAccountTypes);
        }

        public IHttpActionResult GetAccountType(int id)
        {
            var bankAccountTypes = context.BankAccountTypes.SingleOrDefault(a => a.Id == id);
            if (bankAccountTypes == null)
                return NotFound();
            return Ok(Mapper.Map<BankAccountTypeDto>(bankAccountTypes));
        }


        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}
