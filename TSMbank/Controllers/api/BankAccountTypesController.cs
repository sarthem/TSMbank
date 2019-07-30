using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TSMbank.Models;
using TSMbank.Dtos;
using TSMbank.Persistance;

namespace TSMbank.Controllers.api
{
    public class BankAccountTypesController : ApiController
    {
        private readonly ApplicationDbContext context;
        private readonly UnitOfWork unitOfWork;

        public BankAccountTypesController()
        {
            context = new ApplicationDbContext();
            unitOfWork = new UnitOfWork(context);
        }


        // GET /api/bankaccounttypes?description=1
        public IHttpActionResult GetAccountTypes(Description? description = null)
        {
            var bankAccountTypes = from t in context.BankAccountTypes
                                   select t;//2
            
            if (description.HasValue)
            {
                bankAccountTypes = bankAccountTypes.Where(t => t.Description == description);
            }
            
            var bankAccountTypeDtos = bankAccountTypes.ToList().Select(Mapper.Map<BankAccountType,BankAccountTypeDto>);
            return Ok(bankAccountTypeDtos);
        }

        // GET /api/bankaccounttypes/5
        public IHttpActionResult GetAccountType(int id)
        {
            var bankAccountTypes = unitOfWork.BankAccountTypes.GetBankAccountType((byte)(id));//???? //1                
                
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
//1
//context.BankAccountTypes.SingleOrDefault(a => a.Id == id);//edw theloume byte kai fernume int paizei?
//2
//var bankAccoutTypes = unitOfWork.BankAccountTypes.BankAccountType(); ayto den 8a mporuse na antikatastisei tin epanw?
