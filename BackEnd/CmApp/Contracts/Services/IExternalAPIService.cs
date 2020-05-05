using CmApp.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IExternalAPIService
    {
        Task<double> CalculateResult(ExchangeInput input);
        Task<List<string>> GetAvailableCurrencies();
        Task<List<string>> GetAllCountries();
        Task<ExchangeRate> GetSelectedExchangeRate(string name);
        Task<List<string>> GetAllMakeModels(string make);
    }
}
