using CmApp.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CmApp.Contracts
{
    public interface IExchangeRatesRepository
    {
        Task<ExchangeRate> GetLatestForeignExchanges();
        Task<ExchangeRate> GetSelectedExchangeRate(string name);
    }
}
