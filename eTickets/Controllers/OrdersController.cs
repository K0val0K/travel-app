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
                TourId = tourId,
                TourName = tour.Name
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderVM orderVm)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _ordersService.CreateOrderAsync(userId, orderVm);

            return View("OrderCompleted");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _ordersService.GetOrderWithHistoryAndTourInfoAsync(id);

            var orderVM = new OrderVM()
            {
                Id = order.Id,
                TourId = order.TourId,
                TourName = order.Tour.Name,
                ContactEmail = order.ContactEmail,
                ContactPhone = order.ContactPhone,
                PersonQuantity = order.PersonQuantity,
                OrderStatus = order.OrderHistoryItems.OrderByDescending(x => x.CreateDate).First().OrderStatus,
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, OrderVM orderVm)
        {
            if (id != orderVm.Id) return View("NotFound");

            await _ordersService.UpdateOrderAsync(id, orderVm);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var order = await _ordersService.GetOrderWithHistoryAndTourInfoAsync(id);

            var orderVM = new OrderVM()
            {
                Id = order.Id,
            };

            ViewData["CancellationType"] = "Reject";
            return View("Cancel", orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Reject(int id, OrderVM orderVm)
        {
            if (id != orderVm.Id) return View("NotFound");

            await _ordersService.RejectOrderAync(id, orderVm.Comment);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Cancel(int id)
        {
            var order = await _ordersService.GetOrderWithHistoryAndTourInfoAsync(id);

            var orderVM = new OrderVM()
            {
                Id = order.Id,
            };

            ViewData["CancellationType"] = "Cancel";
            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id, OrderVM orderVm)
        {
            if (id != orderVm.Id) return View("NotFound");

            await _ordersService.CancelOrderAsync(id, orderVm.Comment);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Confirm(int id)
        {
            var order = await _ordersService.GetOrderWithHistoryAndTourInfoAsync(id);

            var orderVM = new OrderVM()
            {
                Id = order.Id,
                ContactEmail = order.ContactEmail,
                ContactPhone = order.ContactPhone,
                TourName = order.Tour.Name,
            };

            return View(orderVM);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm(int id, OrderVM orderVm)
        {
            if (id != orderVm.Id) return View("NotFound");

            await _ordersService.ConfirmOrderAsync(id, orderVm.Comment);

            return RedirectToAction("Index");
        }
    }
}
