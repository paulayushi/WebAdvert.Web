using AdvertAPI.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Models.Home;
using WebAdvert.Web.Models.Search;

namespace WebAdvert.Web.ServiceClient
{
    public class CreateAdvertMapperProfile: Profile
    {
        public CreateAdvertMapperProfile()
        {
            CreateMap<CreateAdvertModel, AdvertModel>().ReverseMap();
            CreateMap<CreateAdvertResponse, AdvertResponse>().ReverseMap();
            CreateMap<CreateAdvertConfirmModel, AdvertConfirmModel>().ReverseMap();
            CreateMap<CreateAdvertViewModel, CreateAdvertModel>().ReverseMap();
            CreateMap<AdvertType, SearchViewModel>().ReverseMap();
            CreateMap<AdvertModel, Advertisement>().ReverseMap();
            CreateMap<Advertisement, IndexViewModel>().ReverseMap();
        }
    }
}
