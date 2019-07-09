using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Dtos;
using TSMbank.Models;

namespace TSMbank.App_Start
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<BankAccount, AccountDto>();
            CreateMap<AccountDto, BankAccount>();
            CreateMap<BankAccountType, AccountTypeDto>();
            CreateMap<AccountTypeDto, BankAccountType>();   
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
        }
    }
}