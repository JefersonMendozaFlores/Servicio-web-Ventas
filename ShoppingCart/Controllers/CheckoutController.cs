using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly DataContext _context;
        private UserManager<AppUser> _userManager;

        public CheckoutController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> listadoDeCompras()
        {
            return View(_context.Sales.ToList().OrderByDescending(x => x.Id));
        }

        public IActionResult Tarjeta()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            // Obtener los elementos del carrito de la sesión
            List<CartItem> cart = HttpContext.Session.GetJson<List<CartItem>>("Cart") ?? new List<CartItem>();

            if (cart.Count == 0)
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            // Crear la venta
            var sale = new Sale
            {
                User = await GetUserFromSession(), // Se obitene el usuario
                Datetime = DateTime.Now,
                Total = cart.Sum(item => item.Quantity * item.Price),
                Detail = cart.Select(item => new DetailSale
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price
                }).ToList()
            };

            // Guardar la venta en la base de datos u otra lógica necesaria
            _context.Sales.Add(sale);
            _context.SaveChanges();

            // Vaciar el carrito de la sesión
            HttpContext.Session.Remove("Cart");

            TempData["Success"] = "Purchase successful!";

            return RedirectToAction("listadoDeCompras", "Checkout"); // Redirigir a la página de inicio después de generar la compra
        }

        private async Task<AppUser> GetUserFromSession()
        {
            AppUser user = await _userManager.GetUserAsync(User);

            return user;
        }
    }
}
