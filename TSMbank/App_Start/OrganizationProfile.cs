using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TSMbank.Dtos;
using TSMbank.Models;
using TSMbank.ViewModels;

namespace TSMbank.App_Start
{
    public class OrganizationProfile : Profile
    {
        public OrganizationProfile()
        {
            CreateMap<BankAccount, BankAccountDto>();
            CreateMap<BankAccountDto, BankAccount>();
            CreateMap<BankAccountType, BankAccountTypeDto>();
            CreateMap<BankAccountTypeDto, BankAccountType>();   
            CreateMap<Individual, IndividualDto>();
            CreateMap<IndividualDto, Individual>();
            
        }
    
    }
}