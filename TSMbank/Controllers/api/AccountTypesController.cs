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
    public class AccountTypesController : ApiController
    {
        private ApplicationDbContext context;

        public AccountTypesController()
        {
            context = new ApplicationDbContext();
        }

        public IHttpActionResult GetAccountTypes()
        {
            var accountTypes = context.AccountTypes.ToList().Select(Mapper.Map<AccountType, AccountTypeDto>);
            return Ok(accountTypes);
        }

        public IHttpActionResult GetAccountType(int id)
        {
            var accountType = context.AccountTypes.SingleOrDefault(a => a.Id == id);
            if (accountType == null)
                return NotFound();
            return Ok(Mapper.Map<AccountTypeDto>(accountType));
        }


        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }
    }
}
