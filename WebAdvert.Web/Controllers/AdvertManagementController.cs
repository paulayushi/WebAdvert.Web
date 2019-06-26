using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdvertAPI.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebAdvert.Web.Models.AdvertManagement;
using WebAdvert.Web.Repositories;
using WebAdvert.Web.ServiceClient;

namespace WebAdvert.Web.Controllers
{
    public class AdvertManagementController : Controller
    {
        private readonly IFileUploader _fileUploader;
        private readonly IAdvertApiClient _client;
        private readonly IMapper _mapper;

        public AdvertManagementController(IFileUploader fileUploader, IAdvertApiClient client, IMapper mapper)
        {
            _fileUploader = fileUploader;
            _client = client;
            _mapper = mapper;
        }
        public IActionResult Create()
        {
            var model = new CreateAdvertViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAdvertViewModel model, IFormFile formFile)
        {
            if (ModelState.IsValid)
            {
                var createAdvertModel = _mapper.Map<CreateAdvertModel>(model);
                var advertResponse = await _client.Create(createAdvertModel);
                var id = advertResponse.Id;
                var fileName = "";

                if(formFile != null)
                {
                    fileName = !string.IsNullOrEmpty(formFile.FileName) ? formFile.FileName : id;
                    var filePath = $"{id}/{fileName}";

                    try
                    {
                        using(var fileStream = formFile.OpenReadStream())
                        {
                            var response = await _fileUploader.FileUploader(filePath, fileStream).ConfigureAwait(false);
                            if (!response) throw new Exception("Something went wrong. Could not upload the image to file repository.");
                        }

                        var advertConfirmRequest = new CreateAdvertConfirmModel
                        {
                            Id = id,
                            Status = AdvertStatus.Active
                        };
                        var confirmResponse = await _client.Confirm(advertConfirmRequest).ConfigureAwait(false);
                        if (!confirmResponse)
                        {
                            throw new Exception($"Cannot confirm advert of Id = {id}");
                        }
                        return RedirectToAction("Index", "Home");
                    }
                    catch(Exception ex)
                    {
                        var advertConfirmRequest = new CreateAdvertConfirmModel
                        {
                            Id = id,
                            Status = AdvertStatus.Pending
                        };
                        await _client.Confirm(advertConfirmRequest).ConfigureAwait(false);
                        Console.WriteLine(ex);
                    }
                }
            }
            return View(model);
        }
    }
}