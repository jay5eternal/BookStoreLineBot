using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace BookstoreBot.Models
{
    public class Customer
    {

        public int CustomerId { get; set; }


        public string CustomerName { get; set; }


        public string CustomerPhone { get; set; }


        public DateTime CustomerBirth { get; set; }

        public string CustomerAddress { get; set; }

        public string CustomerEmail { get; set; }

        public string CustomerAccount { get; set; }

      
        public string CustomerPassword { get; set; }


        public string ConfirmPassword { get; set; }

        public bool EmailConfirmed { get; set; }
    }

    public class CustomerViewModel
    {

        public int CustomerId { get; set; }


        public string CustomerName { get; set; }


        public string CustomerPhone { get; set; }


        public DateTime CustomerBirth { get; set; }


        public string CustomerAddress { get; set; }


        public string CustomerEmail { get; set; }


        public string CustomerAccount { get; set; }


        public bool EmailConfirmed { get; set; }
    }


}