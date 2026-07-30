using AspNetCoreGeneratedDocument;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingCart.DataAccess.Data;
using System.Security.Claims;

namespace ShoppingCart.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CartController(ApplicationDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cartItem = _context.CartItems.Where(c => c.UserId == userId).
                Include(x => x.Product).ToList();

            return View(cartItem);
        }
        public async Task<IActionResult> Plus(int cartid)
        {
            var cart = _context.CartItems.FirstOrDefault(u => u.Id == cartid);
            if (cart == null)
            {
                return NotFound();
            }
            cart.Quantity += 1;
            _context.CartItems.Update(cart);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cart Item incremented successfully";

            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Minus(int cartid)
        {
            var cart = _context.CartItems.FirstOrDefault(u => u.Id == cartid);
            if (cart == null)
            {
                return NotFound();
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (cart.Quantity <= 1)
            {
                _context.CartItems.Remove(cart);
                await _context.SaveChangesAsync();
                var count = _context.CartItems.Count(x => x.UserId == userid);
                HttpContext.Session.SetInt32(SD.SessionCart, count);

            }
            else
            {
                cart.Quantity -= 1;
                _context.CartItems.Update(cart);
                await _context.SaveChangesAsync();

            }

            TempData["Success"] = "Cart Item decremented successfully";

            return RedirectToAction(nameof(Index));


        }
        public async Task<IActionResult> Remove(int cartid)
        {
            var cart = _context.CartItems.FirstOrDefault(u => u.Id == cartid);
            if (cart == null)
            {
                return NotFound();
            }
            var userid = User.FindFirstValue(ClaimTypes.NameIdentifier);

            _context.CartItems.Remove(cart);
           

            HttpContext.Session.SetInt32(SD.SessionCart, (_context.CartItems.Count(x => x.UserId == userid)) - 1);
            //it should be before savechanges because after updating cart cart will be 0 and then again we did _1 then it will display -1
            await _context.SaveChangesAsync();
            TempData["Success"] = "Cart Item Removed successfully";

            return RedirectToAction(nameof(Index));


        }
    }
}
