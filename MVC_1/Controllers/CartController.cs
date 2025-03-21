﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MVC_1.Models;
using MVC_1.Repositories.Products;
using MVC_1.Services;
using MVC_1.Settings;
using MVC_1.ViewModels;



namespace MVC_1.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository _productRepository;

        public CartController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public IActionResult Index()
        {
            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            cartItems ??= [];

            var products = _productRepository
                .GetAll()
                .Include(p => p.Category)
                .Where(p => cartItems.Select(i => i.ProductId).Contains(p.Id))
                .ToList();

            // Products quantity
            foreach (var product in products)
            {
                product.QuantityInCart = cartItems.First(i => i.ProductId == product.Id).Quantity;
            }

            var cartVm = new CartVM
            {
                Products = products
            };

            return View(cartVm);
        }

        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
                return BadRequest();

            var items = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            items ??= new List<CartItemVM>();

            if (items.Select(i => i.ProductId).Contains(viewModel.ProductId))
                return BadRequest();

            var newItems = items.ToList();
            newItems.Add(new CartItemVM { ProductId = viewModel.ProductId });
            HttpContext.Session.Set<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey, newItems);

            return Ok();
        }

        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
                return BadRequest();

            var items = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);

            if (items == null)
                return BadRequest();

            if (!items.Select(i => i.ProductId).Contains(viewModel.ProductId))
                return BadRequest();

            items = items.Where(i => i.ProductId != viewModel.ProductId);
            HttpContext.Session.Set(SessionSettings.SessionCartKey, items);

            return Ok();
        }

        public async Task<IActionResult> RemoveFromCartintoCart([FromBody] CartItemVM viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ProductId))
            {
                return RedirectToAction("Index");
            }

            var cartItems = HttpContext.Session.Get<List<CartItemVM>>(SessionSettings.SessionCartKey) ?? new List<CartItemVM>();

            var item = cartItems.FirstOrDefault(i => i.ProductId == viewModel.ProductId);
            if (item == null)
            {
                return RedirectToAction("Index");
            }

            var product = await _productRepository.FindByIdAsync(viewModel.ProductId);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            product.InCart = false;

            cartItems.Remove(item);

            HttpContext.Session.Set(SessionSettings.SessionCartKey, cartItems);

            return RedirectToAction("Index");
        }

        public IActionResult IncreaseQuantity(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            if (cartItems == null)
            {
                return RedirectToAction("Index");
            }

            var item = cartItems.FirstOrDefault(i => i.ProductId == id);
            var product = _productRepository.FindByIdAsync(id).Result;

            if (item != null && item.Quantity < product?.Amount)
            {
                item.Quantity++;
            }

            HttpContext.Session.Set(SessionSettings.SessionCartKey, cartItems);
            return RedirectToAction("Index");
        }

        public IActionResult DecreaseQuantity(string? id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return RedirectToAction("Index");
            }

            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            if (cartItems == null)
            {
                return RedirectToAction("Index");
            }

            var item = cartItems.FirstOrDefault(i => i.ProductId == id);
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
            }

            HttpContext.Session.Set(SessionSettings.SessionCartKey, cartItems);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            if (cartItems == null || cartItems.Count() == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var products = _productRepository
                .GetAll()
                .AsNoTracking()
                .Where(p => cartItems.Select(i => i.ProductId).Contains(p.Id))
                .ToList();

            foreach (var product in products)
            {
                product.Amount -= cartItems.First(i => i.ProductId == product.Id).Quantity;
            }

            await _productRepository.UpdateRangeAsync(products);
            HttpContext.Session.Set<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey, new List<CartItemVM>());

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> СlearCart()
        {
            var cartItems = HttpContext.Session.Get<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey);
            if (cartItems == null || cartItems.Count() == 0)
            {
                return RedirectToAction("Index", "Home");
            }

            var products = _productRepository
                .GetAll()
                .AsNoTracking()
                .Where(p => cartItems.Select(i => i.ProductId).Contains(p.Id))
                .ToList();

            await _productRepository.UpdateRangeAsync(products);
            HttpContext.Session.Set<IEnumerable<CartItemVM>>(SessionSettings.SessionCartKey, new List<CartItemVM>());

            return RedirectToAction("Index", "Home");
        }
    }
}
