using MVC_1.Models;

namespace MVC_1.ViewModels
{
    public class CartVM
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public decimal ProductsPrice
        {
            get
            {
                var prices = Products.Select(p => p.Price * p.QuantityInCart);
                decimal sum = prices.Aggregate(0M, (res, i) => (res + i));
                return sum;
            }
        }
        public decimal TotalPrice { get => ProductsPrice + Shipping; }
        public decimal Shipping { get; set; } = 100;
    }
}
