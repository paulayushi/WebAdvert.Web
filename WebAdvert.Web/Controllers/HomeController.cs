using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models;
using WebAdvert.Web.Models.Home;
using WebAdvert.Web.Models.Search;
using WebAdvert.Web.ServiceClient;

namespace WebAdvert.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ISearchApiClient _searchApiClient;
        private readonly IMapper _mapper;
        private readonly IAdvertApiClient _advertApiClient;

        public HomeController(ISearchApiClient searchApiClient, IMapper mapper, IAdvertApiClient advertApiClient)
        {
            _searchApiClient = searchApiClient;
            _mapper = mapper;
            _advertApiClient = advertApiClient;
        }
        [Authorize]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Index()
        {
            var addsAll = await _advertApiClient.GetAll();
            var allViewModels = addsAll.Select(item => _mapper.Map<IndexViewModel>(item)).ToList();
            return View(allViewModels);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Search(string keyword)
        {
            var searchViewModel = new List<SearchViewModel>();
            var searchResult = await _searchApiClient.GetSearch(keyword);
            if(searchResult != null)
            {
                searchResult.ForEach(searchDoc =>
                {
                    var advertViewModel = _mapper.Map<SearchViewModel>(searchDoc);
                    searchViewModel.Add(advertViewModel);
                });
            }

            return View("Search", searchViewModel);
        }
    }
}
