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
            CreateMap<Account, AccountDto>();
            CreateMap<AccountDto, Account>();
            CreateMap<AccountType, AccountTypeDto>();
            CreateMap<AccountTypeDto, AccountType>();   
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
        }
    }
}