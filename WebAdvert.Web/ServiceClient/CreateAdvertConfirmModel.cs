using AdvertAPI.Models;

namespace WebAdvert.Web.ServiceClient
{
    public class CreateAdvertConfirmModel
    {
        public string Id { get; set; }
        public AdvertStatus Status { get; set; }
    }
}
