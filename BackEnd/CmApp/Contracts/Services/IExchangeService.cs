using CmApp.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IExchangeService
    {
        Task<double> CalculateResult(ExchangeInput input);
        Task<List<string>> GetAvailableCurrencies();
        Task<ExchangeRate> GetSelectedExchangeRate(string name);
    }
}
