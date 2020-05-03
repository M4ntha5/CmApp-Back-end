using CmApp.Domains;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IExchangeService
    {
        Task<double> CalculateResult(ExchangeInput input);
        Task<List<string>> GetAvailableCurrencies();
        Task<List<string>> GetAllCountries();
        Task<ExchangeRate> GetSelectedExchangeRate(string name);
    }
}
