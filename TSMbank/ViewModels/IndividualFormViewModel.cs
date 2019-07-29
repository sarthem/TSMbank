using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public class IndividualFormViewModel
    {
        
        public Individual Individual { get; set; }
        public List<Phone> Phones { get; set; }        
        public Address PrimaryAddress { get; set; }
        public Address SecondaryAddress { get; set; }
        public string IndividualId { get; set; }
        public ModificationAction ModificationAction { get; set; }

        public string Title
        {
            get
            {
                switch (ModificationAction)
                {
                    case ModificationAction.NewIndividual:
                        return "New Customer";
                    case ModificationAction.EditIndividual:
                        return "Edit Customer Personal Info";
                    case ModificationAction.EditAddresses:
                        return "Edit Customer Addresses";
                    case ModificationAction.EditPhones:
                        return "Edit Customer Phone Numbers";
                    default:
                        return "Unrecognized Action";
                }
            }
        }

        public IndividualFormViewModel(Individual individual, List<Phone> phones, Address primaryAddress)
        {
            Individual = individual;
            Phones = phones;
            PrimaryAddress = primaryAddress;           

        }
        public IndividualFormViewModel(Individual individual)
        {
            Individual = individual;
            Phones = individual.Phones.ToList();
            PrimaryAddress = individual.PrimaryAddress;
            SecondaryAddress = individual.SecondaryAddress;
            Individual = individual;
            IndividualId = individual.Id;
        }

        public IndividualFormViewModel(IndividualFormViewModel model)
        {
            Individual = model.Individual;
            Phones = model.Phones;
            PrimaryAddress = model.PrimaryAddress;
        }

        public IndividualFormViewModel()
        {}
    }
}