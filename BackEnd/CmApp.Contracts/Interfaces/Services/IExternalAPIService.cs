using CmApp.Contracts.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts.Interfaces.Services
{
    public interface IExternalAPIService
    {
        Task<double> CalculateResult(ExchangeInput input);
        Task<List<string>> GetAvailableCurrencies();
        Task<List<string>> GetAllCountries();
        Task<ExchangeRate> GetSelectedExchangeRate(string name);
    }
}
