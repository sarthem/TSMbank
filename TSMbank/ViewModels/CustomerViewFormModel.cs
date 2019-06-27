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
        EditAddress,
        EditPhone
    }

    public class CustomerViewFormModel
    {
        
        public Customer Customer { get; set; }
        public Phone Phone { get; set; }
        public Address Address { get; set; }
        public int CustomerId { get; set; }
        public ModificationAction ModificationAction { get; set; }

        //public CustomerViewFormModel()
        //{
        //    //Customer.ModifyAcc = (int) ModifyAccount.NewCustomer;
        //    Customer.CreatedDate = DateTime.Now;
        //    Customer.IsActive = false;
        //}
    }
}