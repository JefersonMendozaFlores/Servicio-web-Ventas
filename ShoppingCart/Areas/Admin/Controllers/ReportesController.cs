using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using ShoppingCart.Infrastructure;
using ShoppingCart.Models;
using ShoppingCart.Models.ViewModels.Report;
using System.Diagnostics;

namespace ShoppingCart.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ReportesController : Controller
    {

        private readonly DataContext _context;
        public ReportesController(DataContext context)
        {
            _context = context;
        }

        public IActionResult resumenDetailSale()
        {


            List<DetailSale> Lista = (from DetailSale in _context.Sales
                                      group DetailSale by DetailSale.Id into grupo
                                      orderby grupo.Count() descending
                                      select new DetailSale
                                      {
                                          Id = grupo.Key,
                                          Quantity = grupo.Count(),
                                      }).ToList();



            return StatusCode(StatusCodes.Status200OK, Lista);
        }



        public IActionResult resumenProduct()
        {

            List<Product> Lista = (from Products in _context.Products
                                   group Products by Products.Name into grupo
                                   orderby grupo.Count() descending
                                   select new Product
                                   {
                                       Name = grupo.Key,
                                       Price = grupo.Count(),
                                   }).Take(4).ToList();

            return StatusCode(StatusCodes.Status200OK, Lista);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReporteGrafic()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public JsonResult GetTotalVentasClientes()
        {
            var lista = from sale in _context.Sales.Include( s => s.User)
                        group sale by sale.User.UserName
                        into g
                        select new 
                        { 
                            Client = g.Key,
                            Count = g.Count() 
                        };



            List<TotalVentasClientes> data = lista.Select(item => new TotalVentasClientes
            {
                name = item.Client,
                y = item.Count
            }).ToList();



            return Json(data);
        }
    }





}
