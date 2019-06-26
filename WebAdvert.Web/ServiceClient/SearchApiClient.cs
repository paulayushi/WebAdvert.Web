using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Search;
using WebAdvert.Web.ServiceClient;

namespace WebAdvert.Web.ServiceClient
{
    public class SearchApiClient : ISearchApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public SearchApiClient(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<List<AdvertType>> GetSearch(string keyword)
        {
            var result = new List<AdvertType>();
            var url = _configuration.GetSection("SearchApi").GetValue<string>("BaseUrl");

            var response = await _httpClient.GetAsync(url + "/" + keyword).ConfigureAwait(false);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var adverts = await response.Content.ReadAsAsync<List<AdvertType>>().ConfigureAwait(false);
                result.AddRange(adverts);
            }
            return result;
        }
    }
}
