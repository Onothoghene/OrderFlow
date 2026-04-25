using Application.DTOs.Restaurants;

namespace Application.DTOs.Courier
{
    public class CourierVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public bool HasRestaurant { get; set; }
        //public RestaurantVM Restaurant { get; set; }

    }

    public class CourierIM
    {
        public string Name { get; set; }
        public int RestaurantId { get; set; }
    }

    public class CourierEM : CourierIM
    {
        public int Id { get; set; }
    }

}
