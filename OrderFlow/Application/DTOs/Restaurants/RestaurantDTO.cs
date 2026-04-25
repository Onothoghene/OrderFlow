using Application.DTOs.Courier;

namespace Application.DTOs.Restaurants
{
    public class RestaurantVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }
        public int CourierId { get; set; }
        public CourierVM Courier { get; set; }
        public bool HasCourier { get; set; }

    }
    
    public class RestaurantIM
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsAvailable { get; set; }
        public int CourierId { get; set; }
    }

    public class RestaurantEM : RestaurantIM
    {
        public int Id { get; set; }
    }

}
