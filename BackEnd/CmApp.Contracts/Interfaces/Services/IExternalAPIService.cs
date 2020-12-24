using CmApp.Contracts.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IExternalAPIService
    {
        Task<decimal> CalculateResult(ExchangeInput input);
        Task<List<string>> GetAvailableCurrencies();
        Task<List<string>> GetAllCountries();
        Task<ExchangeRate> GetSelectedExchangeRate(string name);
    }
}
