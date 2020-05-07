using CodeMash.Models;

namespace CmApp.Entities
{
    [Collection("Shipping")]
    public class ShippingEntity : Entity
    {
        [Field("customs")]
        public double Customs { get; set; } = 0;
        [Field("auction_fee")]
        public double AuctionFee { get; set; } = 0;
        [Field("transfer_fee")]
        public double TransferFee { get; set; } = 0;
        [Field("transportation_fee")]
        public double TransportationFee { get; set; } = 0;
        [Field("car")]
        public string Car { get; set; }

        public string BaseCurrency { get; set; }
        public string CustomsCurrency { get; set; }
        public string AuctionFeeCurrency { get; set; }
        public string TransferFeeCurrency { get; set; }
        public string TransportationFeeCurrency { get; set; }

    }
}
