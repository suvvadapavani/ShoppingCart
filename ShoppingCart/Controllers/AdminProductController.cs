using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    public class AdminProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminProductController(ApplicationDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
                
        }
        [HttpGet]
        public IActionResult Index()
        {
            var products = _context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        [HttpPut]
        public async Task<IActionResult> EditProduct(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            return View(product);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id) 
        {
            Product product = await _context.Products.FindAsync(id);
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product product)
        {
            if (product.ImageFile != null && product.ImageFile.Length > 0)
            {
                //get the wwroot path from environment
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                //clear  and generate a unique file name
                //string originalFilename=Path.GetFileNameWithoutExtension(product.ImageFile.FileName)
                //    .Replace("","_");//removes spaces 
                string originalFilename = Path.GetFileNameWithoutExtension(product.ImageFile.FileName)
                   .Trim()              // removes leading/trailing spaces
                .Replace(" ", "_");  // replaces spaces inside the name

                string extension =Path.GetExtension(product.ImageFile.FileName);
                string uniqueFileName = $"{originalFilename}_{Guid.NewGuid():N}{extension}";
                //ensure that image folder exists
                string imageFolder = Path.Combine(wwwRootPath, "images");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }
                //path to save the image physically
                string filePath = Path.Combine(imageFolder, uniqueFileName);
                //save the file to server
                using(var stream=new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }
                //save relative path (for razor <image src=...>
                product.ImagePath = "/images/" + uniqueFileName;

                //optional Verify file was saved useful for debugging
                string confirmPath=Path.Combine(wwwRootPath,product.ImagePath.TrimStart('/'));
                if (!System.IO.File.Exists(confirmPath))
                {
                    throw new FileNotFoundException("image was not saved correctly", confirmPath);
                }


             }

            if (ModelState.IsValid)
            {
                await _context.Products.AddAsync(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

    }
}
