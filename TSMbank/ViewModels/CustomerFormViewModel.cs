using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TSMbank.Models;

namespace TSMbank.ViewModels
{
    public enum ModificationAction
    {
        NewCustomer,
        EditCustomer,
        EditAddresses,
        EditPhones
    }

    public class CustomerFormViewModel
    {
        
        public Customer Customer { get; set; }
        public List<Phone> Phones { get; set; }
        public Address PrimaryAddress { get; set; }
        public Address SecondaryAddress { get; set; }
        public int CustomerId { get; set; }
        public ModificationAction ModificationAction { get; set; }

        public string Title
        {
            get
            {
                switch (ModificationAction)
                {
                    case ModificationAction.NewCustomer:
                        return "New Customer";
                    case ModificationAction.EditCustomer:
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
    }
}