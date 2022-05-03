using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    public class TravelAgenciesController : Controller
    {
        private readonly ITravelAgenciesService _service;

        public TravelAgenciesController(ITravelAgenciesService service)
        {
            _service = service;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allTravelAgencies = await _service.GetAllAsync();
            return View(allTravelAgencies);
        }


        //Get: TravelAgencies/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Logo,Name,Description")]TravelAgency travelAgency)
        {
            if (!ModelState.IsValid) return View(travelAgency);
            await _service.AddAsync(travelAgency);
            return RedirectToAction(nameof(Index));
        }

        //Get: TravelAgencies/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var travelAgencyDetail = await _service.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");
            return View(travelAgencyDetail);
        }

        //Get: TravelAgencies/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var travelAgencyDetail = await _service.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");
            return View(travelAgencyDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] TravelAgency travelAgency)
        {
            if (!ModelState.IsValid) return View(travelAgency);
            await _service.UpdateAsync(id, travelAgency);
            return RedirectToAction(nameof(Index));
        }

        //Get: TravelAgencies/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var travelAgencyDetail = await _service.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");
            return View(travelAgencyDetail);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var travelAgencyDetail = await _service.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");

            await _service.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
