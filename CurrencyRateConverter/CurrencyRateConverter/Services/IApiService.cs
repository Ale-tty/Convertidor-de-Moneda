using CurrencyRateConverter.Models;
using System.Threading.Tasks;

namespace CurrencyRateConverter.Services
{

    public interface IApiService
    {
        Task<Response> GetCurrencyAsync(string urlBase, string Fecha, string Now, string Base);
    }
}
