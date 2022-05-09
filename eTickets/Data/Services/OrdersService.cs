using eTickets.Data.Enums;
using eTickets.Data.ViewModels;
using eTickets.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly AppDbContext _context;
        public OrdersService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole)
        {
            var orders = await _context.Orders.Include(n => n.OrderHistoryItems).Include(n => n.Tour).Include(n => n.User).ToListAsync();

            if(userRole != "Admin")
            {
                orders = orders.Where(n => n.UserId == userId).ToList();
            }

            return orders;
        }

        public async Task CreateOrderAsync(string userId, OrderVM orderVm)
        {
            var order = new Order()
            {
                UserId = userId,
                TourId = orderVm.TourId,
                PersonQuantity = orderVm.PersonQuantity,
                ContactPhone = orderVm.ContactPhone,
                ContactEmail = orderVm.ContactEmail,
            };

            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var tourOrderHistoryItem = new OrderHistoryItem()
            {
                OrderStatus = Enums.OrderStatus.Pending,
                Comment = orderVm.Comment,
                CreateDate = DateTime.Now,
                OrderId = order.Id,
            };

            await _context.OrderHistoryItems.AddAsync(tourOrderHistoryItem);
            await _context.SaveChangesAsync();

        }

        public async Task<Order> GetOrderWithHistoryAndTourInfoAsync(int orderId)
        {
            var orders = await _context.Orders.Include(n => n.OrderHistoryItems).Include(n => n.Tour).ToListAsync();

            return orders.FirstOrDefault(x => x.Id == orderId);
        }

        public async Task UpdateOrderAsync(int orderId, OrderVM orderVm)
        {
            var dbOrder = await _context.Orders.Include(n => n.OrderHistoryItems).FirstOrDefaultAsync(n => n.Id == orderId);

            if(dbOrder != null)
            {
                dbOrder.PersonQuantity = orderVm.PersonQuantity;
                dbOrder.ContactPhone = orderVm.ContactPhone;
                dbOrder.ContactEmail = orderVm.ContactEmail;
                await _context.SaveChangesAsync();

                var tourOrderHistoryItem = new OrderHistoryItem()
                {
                    OrderStatus = dbOrder.OrderHistoryItems.OrderBy(x => x.CreateDate).Last().OrderStatus,
                    Comment = orderVm.Comment,
                    CreateDate = DateTime.Now,
                    OrderId = dbOrder.Id,
                };

                await _context.OrderHistoryItems.AddAsync(tourOrderHistoryItem);
                await _context.SaveChangesAsync();
            };
        }

        public async Task RejectOrderAync(int orderId, string commentary)
        {
            var dbOrder = await _context.Orders.FirstOrDefaultAsync(n => n.Id == orderId);

            if(dbOrder != null)
            {
                var tourOrderHistoryItem = new OrderHistoryItem()
                {
                    OrderStatus = OrderStatus.Rejected,
                    Comment = commentary,
                    CreateDate = DateTime.Now,
                    OrderId = orderId,
                };

                await _context.OrderHistoryItems.AddAsync(tourOrderHistoryItem);
                await _context.SaveChangesAsync();
            }
            
        }

        public async Task CancelOrderAsync(int orderId, string commentary)
        {
            var dbOrder = await _context.Orders.FirstOrDefaultAsync(n => n.Id == orderId);

            if (dbOrder != null)
            {
                var tourOrderHistoryItem = new OrderHistoryItem()
                {
                    OrderStatus = OrderStatus.Cancelled,
                    Comment = commentary,
                    CreateDate = DateTime.Now,
                    OrderId = orderId,
                };

                await _context.OrderHistoryItems.AddAsync(tourOrderHistoryItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ConfirmOrderAsync(int orderId, string commentary)
        {
            var dbOrder = await _context.Orders.FirstOrDefaultAsync(n => n.Id == orderId);

            if (dbOrder != null)
            {
                var tourOrderHistoryItem = new OrderHistoryItem()
                {
                    OrderStatus = OrderStatus.Confirmed,
                    Comment = commentary,
                    CreateDate = DateTime.Now,
                    OrderId = orderId,
                };

                await _context.OrderHistoryItems.AddAsync(tourOrderHistoryItem);
                await _context.SaveChangesAsync();
            }
        }
    }
}
