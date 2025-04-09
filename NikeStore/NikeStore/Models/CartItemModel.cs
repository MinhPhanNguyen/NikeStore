namespace NikeStore.Models
{
    public class CartItemModel
    {
        public long ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductColor { get; set; }
        public string ProductSize { get; set; }
        public double? ProductPromotion { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice
        {
            get { return Quantity * Price; }
        }
        public string Image { get; set; }
        public CartItemModel()
        {

        }
        public CartItemModel(Product product)
        {
            ProductId = product.ProductID;
            ProductName = product.Name;
            Price = product.Price;
            ProductColor = product.ProductColor?.Color;
            ProductSize = product.ProductSize?.Size;
            Quantity = 1;
            Image = product.Images?.FirstOrDefault().ImageUrl ?? "default.png";
        }
    }
}
