using CuStore.Domain.Abstract;
using CuStore.Domain.Entities;
using CuStore.WebUI.Infrastructure.Helpers;
using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CuStore.WebUI.Infrastructure.Abstract;
using CuStore.WebUI.Infrastructure.Implementations;
using static CuStore.WebUI.Controllers.ManageController;

namespace CuStore.WebUI.Controllers
{
    public class UserAddressController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ICountriesProvider _countriesProvider;
        private readonly IPlacesApiClient _placesApiClient;

        public UserAddressController(IUserRepository userRepository, IPlacesApiClient placesApiClient, ICountriesProvider countriesProvider)
        {
            _userRepository = userRepository;
            _countriesProvider = countriesProvider;
            _placesApiClient = placesApiClient;
        }

        public ViewResult Edit()
        {
            var userAddress = _userRepository.GetUserAddress(this.User.Identity.GetUserId()) ?? new UserAddress
            {
                UserId = User.Identity.GetUserId()
            };

            ViewBag.CountryList = _countriesProvider.FillCountryList();

            return View(userAddress);
        }

        [HttpPost]
        public ActionResult Edit(UserAddress userAddress)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.AddOrSaveUserAddress(userAddress))
                {
                    return RedirectToAction("Index", "Manage", new { Message = ManageMessageId.EditAddressSuccess });
                }
            }

            ViewBag.CountryList = _countriesProvider.FillCountryList();
            ModelState.AddModelError("", @"Given address is invalid. Please correct errors.");
            return View(userAddress);
        }

        [HttpGet]
        public async Task<JsonResult> GetCities(string cityPrefix, string country = null)
        {
            IPlacesApiClient client = new GoogleMapsApiClient(_countriesProvider);
            List<string> cities = await client.GetCities(cityPrefix, country);
            return Json(cities, JsonRequestBehavior.AllowGet);
        }
    }
}