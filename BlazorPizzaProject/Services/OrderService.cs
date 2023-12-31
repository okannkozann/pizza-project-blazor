﻿using Blazored.LocalStorage;
using BlazorPizzaProject.Business.DataModels;
using BlazorPizzaProject.Business.Models;

namespace BlazorPizzaProject.Services
{
    public class OrderService : IOrderService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly HttpClient _httpClient;

        public OrderService(ILocalStorageService localStorageService, HttpClient httpClient)
        {
            _localStorageService = localStorageService;
            _httpClient = httpClient;
        }

        public int Count { get; set; }

        public event Action? OnChange;

        public async Task<Response> AddOrder(OrderModel model)
        {
            var orderModel = new OrderModel { PizzaId = model.PizzaId, PizzaPrice = Convert.ToDouble(model.PizzaPrice) };

            var MyOrder = await _localStorageService.GetItemAsync<List<OrderModel>>("MyOrder");
            if (MyOrder == null)
            {
                MyOrder = new List<OrderModel> { orderModel };
                await _localStorageService.SetItemAsync("MyOrder", MyOrder);
                await InvokeAndCount();
                return new Response { Success = true, Message = "Order created successfully" };

            }
            else
            {
                var checkEx = MyOrder.FirstOrDefault(i => i.PizzaId == orderModel.PizzaId);
                if (checkEx != null)
                {
                    if (checkEx.PizzaPrice != orderModel.PizzaPrice)
                    {
                        MyOrder.Remove(checkEx);
                        MyOrder.Add(orderModel);
                        await _localStorageService.SetItemAsync("MyOrder", MyOrder);
                        await InvokeAndCount();
                        return new Response { Success = true, Message = "Order updated successfully" };
                    }
                }
                else
                {
                    MyOrder.Add(orderModel);
                    await _localStorageService.SetItemAsync("MyOrder", MyOrder);
                    await InvokeAndCount();
                    return new Response { Success = true, Message = "Order created successfully" };
                }
            }

            return new Response { Success = false, Message = "Order Added already" };
        }

        private async Task InvokeAndCount()
        {
            Count = (await _localStorageService.GetItemAsync<List<OrderModel>>("MyOrder")).Count();
            OnChange?.Invoke();
        }

        public async Task<int> RefreshCount()
        {
            try
            {
                Count = (await _localStorageService.GetItemAsync<List<OrderModel>>("MyOrder")).Count();
                OnChange?.Invoke();
                return Count;
            }
            catch (Exception ex) { return 0; }

        }

        public async Task<List<MyOrdersModel>> GetOrders()
        {
            var myOrdersList = new List<MyOrdersModel>();
            var myOrders = await _localStorageService.GetItemAsync<List<OrderModel>>("MyOrder");

            if (myOrders != null && myOrders.Count() > 0)
            {
                foreach (var item in myOrders)
                {
                    var r = await _httpClient.GetFromJsonAsync<Pizza>($"api/pizza/{item.PizzaId}");
                    if (r != null)
                    {
                        if (Convert.ToDouble(r.SmallSize) == Convert.ToDouble(item.PizzaPrice)
                        || Convert.ToDouble(r.MediumSize) == Convert.ToDouble(item.PizzaPrice)
                        || Convert.ToDouble(r.LargeSize) == Convert.ToDouble(item.PizzaPrice)
                        || Convert.ToDouble(r.ExtraLargeSize) == Convert.ToDouble(item.PizzaPrice))
                        {
                            string sizeName = string.Empty;
                            if (Convert.ToDouble(r.SmallSize) == Convert.ToDouble(item.PizzaPrice))
                            {
                                sizeName = "Small";
                            }
                            else if (Convert.ToDouble(r.MediumSize) == Convert.ToDouble(item.PizzaPrice))
                            {
                                sizeName = "Medium";
                            }
                            else if (Convert.ToDouble(r.LargeSize) == Convert.ToDouble(item.PizzaPrice))
                            {
                                sizeName = "Large";
                            }
                            else if (Convert.ToDouble(r.ExtraLargeSize) == Convert.ToDouble(item.PizzaPrice))
                            {
                                sizeName = "Extra Large";
                            }

                            var myOrderlist = new MyOrdersModel
                            {
                                PizzaId = r.Id,
                                Name = r.Name,
                                Image = r.Image,
                                Price = item.PizzaPrice,
                                SizeName = sizeName
                            };
                            myOrdersList.Add(myOrderlist);
                        }
                    }
                }
                return myOrdersList;
            }
            return null!;
        }

        public async Task<Response> DeleteOrder(int id)
        {
            var myOrders = await _localStorageService.GetItemAsync<List<OrderModel>>("MyOrder");
            var r = myOrders.FirstOrDefault(i => i.PizzaId == id);
            if (r != null)
            {
                myOrders.Remove(r);
                await _localStorageService.SetItemAsync("MyOrder", myOrders);
                await InvokeAndCount();
                return new Response { Success = true, Message = "Order deleted successfully" };
            }
            return new Response { Success = false, Message = "Error Occured : Order not found" };
        }
    }
}
