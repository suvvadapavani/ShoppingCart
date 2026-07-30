using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.DataAccess.Data;
using ShoppingCart.Models;

namespace ShoppingCart.Controllers
{
    [Authorize(Roles="Admin")]
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
            //VieData needs converton in order to access
           //total product count it is dynamic type will be taken dynamically based on the value assigned
            ViewBag.TotalProducts = products.Count();
            ViewData["TotalProducts"]= products.Count();
            //latest product
            ViewBag.LatestProduct = products.OrderByDescending(p => p.CreatedDate)
                .Select(p => p.ProductName)
                .FirstOrDefault() ?? "No Products";
            //Max price product
            ViewBag.MaxPrice = products.Any() ? products.Max(p => p.Price) : 0;


            return View(products);
       

        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

     
        public async Task<IActionResult> EditProduct(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            return View(product);
        }
        //[HttpPost]
        //public async Task<IActionResult> EditProduct(Product model)
        //{
        //    var product = await _context.Products.FindAsync(model.Id);

        //    if (product == null)
        //        throw new Exception("Product not found");

        //    product.ProductName = model.ProductName;
        //    product.Description = model.Description;
        //    product.Price = model.Price;
        //    product.ImagePath= model.ImagePath;
        //    product.CreatedDate = DateTime.UtcNow;

        //    _context.Products.Update(product);
        //    await _context.SaveChangesAsync();
        //    TempData["Success"] = "Record updated sucessfully";
        //    return RedirectToAction("Index");
        //}
        [HttpPost]
        public async Task<IActionResult> EditProduct(Product model)
        {
            var product = await _context.Products.FindAsync(model.Id);

            if (product == null)
                throw new Exception("Product not found");

            product.ProductName = model.ProductName;
            product.Description = model.Description;
            product.Price = model.Price;
            product.CreatedDate = DateTime.UtcNow;

            // Handle image upload
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string originalFilename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName)
                    .Trim().Replace(" ", "_");
                string extension = Path.GetExtension(model.ImageFile.FileName);
                string uniqueFileName = $"{originalFilename}_{Guid.NewGuid():N}{extension}";

                string imageFolder = Path.Combine(wwwRootPath, "images");
                if (!Directory.Exists(imageFolder))
                {
                    Directory.CreateDirectory(imageFolder);
                }

                string filePath = Path.Combine(imageFolder, uniqueFileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                product.ImagePath = "/images/" + uniqueFileName;
            }
            else
            {
                // Keep the old image if no new file uploaded
                product.ImagePath = product.ImagePath;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Record updated successfully";
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> DeleteProduct(int id) 
        {
            if (id == null)
            {
                return NotFound();
            }
            Product product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound();
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product!=null)
            {
                //Image remains in wwroot imagaes folder so in order to delete the product along with image we have to write the below if 
                if (!string.IsNullOrEmpty(product.ImagePath))
                {
                    var imagepath = Path.Combine(_webHostEnvironment.WebRootPath, product.ImagePath.TrimStart('/').Replace("/", "\\"));
                    if (System.IO.File.Exists(imagepath))
                    {
                        System.IO.File.Delete(imagepath);
                    }
                }
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
            TempData["Success"] = "Record deleted successfully";

            return RedirectToAction(nameof(Index));

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
                TempData["Success"] = "Record Added successfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(product);
            }
        }

    }
}
