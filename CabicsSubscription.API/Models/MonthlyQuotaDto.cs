namespace CabicsSubscription.API.Models
{
    public class MonthlyQuotaDto : DTO
    {
       public QuoteDetail QuoteDetailResponse { get; set; }
    }

    public class QuoteDetail
    {
        public int RemainingNoOfDriver { get; set; }
        public int RemainingNoOfAgent { get; set; }
        public int RemainingNoOfVehicle { get; set; }

    }
}