using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
    
    public class ProductController : Controller
    {
        public readonly  ApplicationDbContext  _context;
        public ProductController(ApplicationDbContext context)
        {
             _context= context;   
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                HttpContext.Session.SetInt32(SD.SessionCart, _context.CartItems.Where(c => c.UserId == userId).Count());
            }
            var products = _context.Products.ToList();
            return View(products);
        }
        public IActionResult Details(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            return View(product);

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(CartItem cartItem)
        {
            //to get logged in user id
            var userId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartProduct = _context.CartItems.FirstOrDefault(x => x.ProductId == cartItem.ProductId && x.UserId == userId);
            if(cartProduct!=null)
            {
                cartProduct.Quantity += cartItem.Quantity;
                _context.CartItems.Update(cartProduct);
                await _context.SaveChangesAsync();
            }
            else
            {
                cartItem.Id = 0;
                cartItem.UserId = userId;

                _context.CartItems.Add(cartItem);
                await _context.SaveChangesAsync();
            
            }
            return RedirectToAction("Index");


        }
    }
}
