using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace PaymentClaimApi.Models
{
    public class ClaimDetails
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber {get; set;}
        public string Type {get; set;}
        public string Date {get; set;} 
        public string Amount {get; set;}
        public string Currency {get;set;}
        public string Description {get;set;}

        public string Status {get;set;}
    }
}
