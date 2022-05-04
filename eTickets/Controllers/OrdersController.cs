using eTickets.Data.Services;
using eTickets.Data.Static;
using eTickets.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eTickets.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IToursService _toursService;
        private readonly IOrdersService _ordersService;

        public OrdersController(IToursService toursService, IOrdersService ordersService)
        {
            _toursService = toursService;
            _ordersService = ordersService;
        }

        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string userRole = User.FindFirstValue(ClaimTypes.Role);

            var orders = await _ordersService.GetOrdersByUserIdAndRoleAsync(userId, userRole);
            return View(orders);
        }

        public async Task<IActionResult> Create(int tourId)
        {
            var tour = await _toursService.GetTourByIdAsync(tourId);

            var orderVM = new OrderVM()
            {
                TourName = tour.Name
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderVM orderVM)
        {
            //Some logic
            return View("OrderCompleted");
        }
    }
}
