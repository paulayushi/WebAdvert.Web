using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AdvertAPI.Models;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace WebAdvert.Web.ServiceClient
{
    public class AdvertApiClient : IAdvertApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _client;
        private readonly IMapper _mapper;

        public AdvertApiClient(IConfiguration configuration, HttpClient client, IMapper mapper)
        {
            _configuration = configuration;
            _client = client;
            _mapper = mapper;

            var baseCreateUrl = _configuration.GetSection("AdvertApi").GetValue<string>("BaseUrl");
            _client.BaseAddress = new Uri(baseCreateUrl);
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");
        }

        public async Task<bool> Confirm(CreateAdvertConfirmModel model)
        {
            var confirmAdvertModel = _mapper.Map<AdvertConfirmModel>(model);
            var jsonModel = JsonConvert.SerializeObject(confirmAdvertModel);

            var response = await _client.PutAsync($"{_client.BaseAddress}/confirm", new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);

            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<AdvertResponse> Create(CreateAdvertModel model)
        {
            var advertModel = _mapper.Map<AdvertModel>(model);

            var jsonModel = JsonConvert.SerializeObject(advertModel);
            var response = await _client.PostAsync($"{_client.BaseAddress}/create", new StringContent(jsonModel, Encoding.UTF8, "application/json")).ConfigureAwait(false);
            var responJson = await response.Content.ReadAsStringAsync();
            var createAdvertResponse = JsonConvert.DeserializeObject<CreateAdvertResponse>(responJson);
            var advertResponse = _mapper.Map<AdvertResponse>(createAdvertResponse);

            return advertResponse;
        }

        public async Task<List<Advertisement>> GetAll()
        {
            var apiResponse = await _client.GetAsync($"{_client.BaseAddress}/all").ConfigureAwait(false);
            var advertResult = await apiResponse.Content.ReadAsAsync<List<AdvertModel>>().ConfigureAwait(false);
            return advertResult.Select(item => _mapper.Map<Advertisement>(item)).ToList();
        }
    }
}
