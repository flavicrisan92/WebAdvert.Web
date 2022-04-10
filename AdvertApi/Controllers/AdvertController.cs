using AdvertApi.Models;
using AdvertApi.Models.Flavius;
using AdvertApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdvertApi.Controllers
{
    [ApiController]
    [Route("api/v1/adverts")]
    public class AdvertController : Controller
    {
        private readonly IAdvertStorageService _adverStorageService;

        public AdvertController(IAdvertStorageService adverStorageService)
        {
            _adverStorageService = adverStorageService;
        }

        public string recordId { get; private set; }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(400)]
        [ProducesResponseType(201, Type = typeof(CreateAdvertResponse))]
        public async Task<IActionResult> Create(AdvertModel model)
        {
            try
            {
                recordId = await _adverStorageService.Add(model);
                return StatusCode(201, new CreateAdvertResponse
                {
                    Id = recordId
                });
            }
            catch (KeyNotFoundException ex)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
                throw;
            }
        }

        [HttpPut]
        [Route("Confirm")]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Confirm(ConfirmAdvertModel model)
        {
            try
            {
                await _adverStorageService.Confirm(model);
            }
            catch (KeyNotFoundException)
            {
                return new NotFoundResult();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

            return new OkResult();
        }
    }
}
