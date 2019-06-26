using System.Collections.Generic;
using System.Threading.Tasks;
using AdvertAPI.Models;

namespace WebAdvert.Web.ServiceClient
{
    public interface IAdvertApiClient
    {
        Task<AdvertResponse> Create(CreateAdvertModel model);
        Task<bool> Confirm(CreateAdvertConfirmModel model);
        Task<List<Advertisement>> GetAll();
    }
}
