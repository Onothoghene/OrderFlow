using Application.DTOs.Address;
using Application.DTOs.Payment;
using Application.DTOs.PersonalDetails;
using Application.DTOs.Restaurants;
using Domain.Entities;
using System.Collections.Generic;

namespace Application.DTOs.Orders
{
    public class OrderVM
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }

        public RestaurantVM Restaurant { get; set; }
        public UserProfile User { get; set; }
        public List<OrderItemsVM> OrderItems { get; set; }

    }
    
    public class OrderIM
    {
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }
        public List<OrderItemsIM> OrderItems { get; set; }

    }

    public class OrderEM : OrderIM
    {
        public int Id { get; set; }
    }


    public class OrderSummaryVM
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public decimal TotalAmount { get; set; }
        public int PaymentStatus { get; set; }
        public int PaymentOption { get; set; }
        public string OrderDate { get; set; }
        public int Status { get; set; }

        public RestaurantVM Restaurant { get; set; }
        public AddressVM Address { get; set; }
        public PersonalDetailsVM User { get; set; }
        public List<OrderItemsVM> OrderItems { get; set; }
        //public List<PaymentVM> Payment { get; set; }
        public PaymentVM Payment { get; set; }

    }
}
