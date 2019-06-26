using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Web.Models.Search;

namespace WebAdvert.Web.ServiceClient
{
    public interface ISearchApiClient
    {
        Task<List<AdvertType>> GetSearch(string keyword);
    }
}
