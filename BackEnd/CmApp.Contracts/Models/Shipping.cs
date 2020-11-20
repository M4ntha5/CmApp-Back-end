using System.ComponentModel.DataAnnotations.Schema;

namespace CmApp.Contracts.Entities
{
    public class Shipping
    {
        public int Id { get; set; }
        public decimal? Customs { get; set; }
        public decimal? AuctionFee { get; set; }
        public decimal? TransferFee { get; set; }
        public decimal? TransportationFee { get; set; }
        public string CustomsCurrency { get; set; }
        public string AuctionFeeCurrency { get; set; }
        public string TransferFeeCurrency { get; set; }
        public string TransportationFeeCurrency { get; set; }

        public virtual Car Car { get; set; }
    }
}
