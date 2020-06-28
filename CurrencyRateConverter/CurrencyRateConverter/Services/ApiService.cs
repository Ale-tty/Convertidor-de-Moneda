using CurrencyRateConverter.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyRateConverter.Services
{
    public class ApiService : IApiService
    {
        public async Task<Response> GetCurrencyAsync(string urlBase, string Fecha, string Now, string Base)
        {
            try
            {
                HttpClient client = new HttpClient
                {
                    BaseAddress = new Uri(urlBase),
                };

                string url = $"history?start_at={Fecha}&end_at={Now}&base={Base}"; //espacios en string
                HttpResponseMessage response = await client.GetAsync(url);
                string result = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new Response
                    {
                        IsSuccess = false,
                        Message = result,
                    };
                }

                ApiObj model = JsonConvert.DeserializeObject<ApiObj>(result);
                return new Response
                {
                    IsSuccess = true,
                    Result = model
                };
            }
            catch (Exception ex)
            {
                return new Response
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
    }
}
