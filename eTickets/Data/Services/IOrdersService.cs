using eTickets.Data.ViewModels;
using eTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface IOrdersService
    {
        //Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);

        Task CreateOrderAsync(string userId, OrderVM orderVm);

        Task<Order> GetOrderWithHistoryAndTourInfoAsync(int orderId);

        Task UpdateOrderAsync(int orderId, OrderVM orderVm);

        Task RejectOrderAync(int orderId, string commentary);

        Task CancelOrderAsync(int orderId, string commentary);

        Task ConfirmOrderAsync(int orderId, string commentary);
    }
}
