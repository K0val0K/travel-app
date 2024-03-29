﻿using eTickets.Data;
using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class ToursController : Controller
    {
        private readonly IToursService _service;
        private readonly IBookmarksService _bookmarksService;
        private readonly ITravelManagerService _travelManagerService;

        public ToursController(IToursService service, IBookmarksService bookmarksService, ITravelManagerService travelManagerService)
        {
            _service = service;
            _bookmarksService = bookmarksService;
            _travelManagerService = travelManagerService;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            var allTours = await _service.GetAllAsync(n => n.TravelAgency);
            if (userRole == UserRoles.Manager)
            {
                var travelAgencyId = await _travelManagerService.GetTravelAgencyIdByManagerUserId(userId);
                allTours = allTours.Where(n => n.TravelAgencyId == travelAgencyId);
            }
           

            //bookmarks check
            var bookmarks = await _bookmarksService.GetUserBookmarksAsync(userId);

            var bookmarkedTours = new List<Tour>();
            foreach(var tour in allTours)
            {
                if(bookmarks.Any(x => x.Tour == tour))
                {
                    bookmarkedTours.Add(tour);
                }
            }
            ViewData["BookmarkedTours"] = bookmarkedTours;
            return View(allTours);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Filter(string searchString)
        {
            var allTours = await _service.GetAllAsync(n => n.TravelAgency);

            if (!string.IsNullOrEmpty(searchString))
            {
                var filteredResultNew = allTours.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();

                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var bookmarks = await _bookmarksService.GetUserBookmarksAsync(userId);

                var bookmarkedTours = new List<Tour>();
                foreach (var tour in allTours)
                {
                    if (bookmarks.Any(x => x.Tour == tour))
                    {
                        bookmarkedTours.Add(tour);
                    }
                }
                ViewData["BookmarkedTours"] = bookmarkedTours;

                //var filteredResultNew = allTours.Where(n => string.Equals(n.Name, searchString, StringComparison.CurrentCultureIgnoreCase) || string.Equals(n.Description, searchString, StringComparison.CurrentCultureIgnoreCase)).ToList();

                return View("Index", filteredResultNew);
            }

            return View("Index", allTours);
        }

        [AllowAnonymous]
        public async Task<IActionResult> SortByPrice()
        {
            var allTours = await _service.GetAllAsync(n => n.TravelAgency);

                //var filteredResult = allTours.Where(n => n.Name.ToLower().Contains(searchString.ToLower()) || n.Description.ToLower().Contains(searchString.ToLower())).ToList();


            var filteredResultNew = allTours.OrderBy(x => x.Price).ToList();

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookmarks = await _bookmarksService.GetUserBookmarksAsync(userId);

            var bookmarkedTours = new List<Tour>();
            foreach (var tour in allTours)
            {
                if (bookmarks.Any(x => x.Tour == tour))
                {
                    bookmarkedTours.Add(tour);
                }
            }
            ViewData["BookmarkedTours"] = bookmarkedTours;

            return View("Index", filteredResultNew);
        }

        //GET: Tours/Details/1
        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var tourDetail = await _service.GetTourByIdAsync(id);
            return View(tourDetail);
        }

        //GET: Tours/Create
        public async Task<IActionResult> Create()
        {
            var tourDropdownsData = await _service.GetNewTourDropdownsValues();
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);
            if (userRole == UserRoles.Manager)
            {
                var travelAgencyId = await _travelManagerService.GetTravelAgencyIdByManagerUserId(userId);
                tourDropdownsData.TravelAgencies.RemoveAll(x => x.Id != travelAgencyId);
            }
            ViewBag.TravelAgencies = new SelectList(tourDropdownsData.TravelAgencies, "Id", "Name");
            ViewBag.Countries = new SelectList(tourDropdownsData.Countries, "Id", "CountryName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewTourVM tour)
        {
            if (!ModelState.IsValid)
            {
                var tourDropdownsData = await _service.GetNewTourDropdownsValues();
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userRole == UserRoles.Manager)
                {
                    var travelAgencyId = await _travelManagerService.GetTravelAgencyIdByManagerUserId(userId);
                    tourDropdownsData.TravelAgencies.RemoveAll(x => x.Id != travelAgencyId);
                }
                ViewBag.TravelAgencies = new SelectList(tourDropdownsData.TravelAgencies, "Id", "Name");
                ViewBag.Countries = new SelectList(tourDropdownsData.Countries, "Id", "CountryName");

                return View(tour);
            }

            await _service.AddNewTourAsync(tour);
            return RedirectToAction(nameof(Index));
        }


        //GET: Tours/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var tourDetails = await _service.GetTourByIdAsync(id);
            if (tourDetails == null) return View("NotFound");

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var response = new NewTourVM()
            {
                Id = tourDetails.Id,
                Name = tourDetails.Name,
                Description = tourDetails.Description,
                Price = tourDetails.Price,
                StartDate = tourDetails.StartDate,
                EndDate = tourDetails.EndDate,
                ImageURL = tourDetails.ImageURL,
                TourCategory = tourDetails.TourCategory,
                TravelAgencyId = tourDetails.TravelAgencyId,
                CountryIds = tourDetails.Countries_Tours.Select(n => n.CountryId).ToList(),
            };

            var tourDropdownsData = await _service.GetNewTourDropdownsValues();
            if (userRole == UserRoles.Manager)
            {
                var travelAgencyId = await _travelManagerService.GetTravelAgencyIdByManagerUserId(userId);
                tourDropdownsData.TravelAgencies.RemoveAll(x => x.Id != travelAgencyId);
            }
            ViewBag.TravelAgencies = new SelectList(tourDropdownsData.TravelAgencies, "Id", "Name");
            ViewBag.Countries = new SelectList(tourDropdownsData.Countries, "Id", "CountryName");

            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, NewTourVM tour)
        {
            if (id != tour.Id) return View("NotFound");

            if (!ModelState.IsValid)
            {
                var tourDropdownsData = await _service.GetNewTourDropdownsValues();
                string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string userRole = User.FindFirstValue(ClaimTypes.Role);
                if (userRole == UserRoles.Manager)
                {
                    var travelAgencyId = await _travelManagerService.GetTravelAgencyIdByManagerUserId(userId);
                    tourDropdownsData.TravelAgencies.RemoveAll(x => x.Id != travelAgencyId);
                }
                ViewBag.TravelAgencies = new SelectList(tourDropdownsData.TravelAgencies, "Id", "Name");
                ViewBag.Countries = new SelectList(tourDropdownsData.Countries, "Id", "CountryName");

                return View(tour);
            }

            await _service.UpdateTourAsync(tour);
            return RedirectToAction(nameof(Index));
        }
    }
}