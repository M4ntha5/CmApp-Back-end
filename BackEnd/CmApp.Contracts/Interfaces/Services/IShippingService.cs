using CmApp.Contracts.Entities;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IShippingService
    {
        Task UpdateShipping(int carId, Shipping shipping);
        Task<Shipping> InsertShipping(int carId, Shipping shipping);
    }
}
