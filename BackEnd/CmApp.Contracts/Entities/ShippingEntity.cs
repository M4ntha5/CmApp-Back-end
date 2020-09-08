namespace CmApp.Contracts.Entities
{
    public class ShippingEntity
    {
        public int ID { get; set; }
        public double Customs { get; set; } = 0;
        public double AuctionFee { get; set; } = 0;
        public double TransferFee { get; set; } = 0;
        public double TransportationFee { get; set; } = 0;
        public int Car { get; set; }
        public string BaseCurrency { get; set; }
        public string CustomsCurrency { get; set; }
        public string AuctionFeeCurrency { get; set; }
        public string TransferFeeCurrency { get; set; }
        public string TransportationFeeCurrency { get; set; }

    }
}
