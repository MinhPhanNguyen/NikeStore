namespace NikeStore.Models.ViewModels
{
    public class WishListViewModel
    {
        public long ProductID { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string CategoryName { get; set; }
        public decimal Price { get; set; }
    }

}
