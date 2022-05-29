using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class CountriesController : Controller
    {
        private readonly ICountriesService _service;

        public CountriesController(ICountriesService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await _service.GetAllAsync();
            return View(data);
        }

        //Get: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("CountryName,CountryPictureURL,Description")]Country country)
        {
            if (!ModelState.IsValid)
            {
                return View(country);
            }
            await _service.AddAsync(country);
            return RedirectToAction(nameof(Index));
        }

        //Get: Countries/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var countryDetails = await _service.GetByIdAsync(id);

            if (countryDetails == null) return View("NotFound");
            return View(countryDetails);
        }

        //Get: Countries/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var countryDetails = await _service.GetByIdAsync(id);
            if (countryDetails == null) return View("NotFound");
            return View(countryDetails);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CountryName,CountryPictureURL,Description")] Country country)
        {
            if (!ModelState.IsValid)
            {
                return View(country);
            }
            await _service.UpdateAsync(id, country);
            return RedirectToAction(nameof(Index));
        }

        //Get: Countries/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");
            return View(actorDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actorDetails = await _service.GetByIdAsync(id);
            if (actorDetails == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
