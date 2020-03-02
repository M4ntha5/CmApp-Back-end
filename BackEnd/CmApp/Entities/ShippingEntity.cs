using CodeMash.Models;

namespace CmApp.Entities
{
    [Collection("Shipping")]
    public class ShippingEntity : Entity
    {
        [Field("customs")]
        public double Customs { get; set; }
        [Field("auction_fee")]
        public double AuctionFee { get; set; }
        [Field("transfer_fee")]
        public double TransferFee { get; set; }
        [Field("transportation_fee")]
        public double TransportationFee { get; set; }
        [Field("car")]
        public string Car { get; set; }
    }
}
