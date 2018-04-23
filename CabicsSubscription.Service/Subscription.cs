﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabicsSubscription.Service
{
    public class Subscription
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int PlanId { get; set; }
        public string PlanName { get; set; }
        public int SubscriptionTypeId { get; set; }
        public int SubcriptionStatusCode { get; set; }
        public double SubscriptionPrice { get; set; }
        public int NoOfSmsPurchase { get; set; }
        public double SMSPrice { get; set; }
        public double TotalPrice { get; set; }
        public int TotalCredit { get; set; }
        public int RemainingCredit { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public Nullable<int> NoOfAgents { get; set; }
        public Nullable<int> NoOfDrivers { get; set; }
        public Nullable<int> NoOfVehicles { get; set; }
        public Nullable<int> RemainingNoOfAgents { get; set; }
        public Nullable<int> RemainingNoOfDrivers { get; set; }
        public Nullable<int> RemainingNoOfVehicles { get; set; }
        public Nullable<double> PerSMSPrice { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }


    }
}