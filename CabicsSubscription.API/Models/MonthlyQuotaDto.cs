namespace CabicsSubscription.API.Models
{
    public class MonthlyQuotaDto : DTO
    {
       
        public SubscriptionMonthly SubscriptionMonthlyResponse { get; set; }
    }

    public class QuoteDetail
    {
        public string SubscriptionType { get; set; }
        public int SubscriptionTypeId { get; set; }
        public SubscriptionMonthly subscriptionMonthly { get; set; }
        public SubscriptionPayAsYouGo subscriptionPayAsYouGo { get; set; }
    }

    public class SubscriptionMonthly
    {
        public bool IsAllowAddition { get; set; }
        public int RemainingNoOfDriver { get; set; }
        public int RemainingNoOfAgent { get; set; }
        public int RemainingNoOfVehicle { get; set; }
    }

    public class SubscriptionPayAsYouGo
    {
        public int TotalCredit { get; set; }
        public int RemainingCredit { get; set; }
    }
}