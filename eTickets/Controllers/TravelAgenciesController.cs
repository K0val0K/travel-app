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
        private readonly ITravelAgenciesService _travelAgencyService;
        private readonly IBookmarksService _bookmarksService;

        public TravelAgenciesController(ITravelAgenciesService service, IBookmarksService bookmarksService)
        {
            _travelAgencyService = service;
            _bookmarksService = bookmarksService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var allTravelAgencies = await _travelAgencyService.GetAllAsync();
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
            await _travelAgencyService.AddAsync(travelAgency);
            return RedirectToAction(nameof(Index));
        }

        //Get: TravelAgencies/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var travelAgencyDetail = await _travelAgencyService.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");
            return View(travelAgencyDetail);
        }

        //Get: TravelAgencies/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var travelAgencyDetail = await _travelAgencyService.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");
            return View(travelAgencyDetail);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Logo,Name,Description")] TravelAgency travelAgency)
        {
            if (!ModelState.IsValid) return View(travelAgency);
            await _travelAgencyService.UpdateAsync(id, travelAgency);
            return RedirectToAction(nameof(Index));
        }

        //Get: TravelAgencies/Delete/1
        public async Task<IActionResult> Delete(int id)
        {
            var travelAgencyDetail = await _travelAgencyService.GetByIdAsync(id);
            if (travelAgencyDetail == null) return View("NotFound");
            return View(travelAgencyDetail);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var travelAgencyDetail = await _travelAgencyService.GetByIdAsync(id, t => t.Tours);
            if (travelAgencyDetail == null) return View("NotFound");

            foreach(var tour in travelAgencyDetail?.Tours)
            {
                await _bookmarksService.DeleteTourBookmarks(tour);
            }

            await _travelAgencyService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
