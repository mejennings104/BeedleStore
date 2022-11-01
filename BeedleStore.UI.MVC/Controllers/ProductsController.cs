using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeedleStore.DATA.EF.Models;
using Microsoft.AspNetCore.Authorization;
using System.Reflection.Metadata.Ecma335;
using System.Drawing;
using BeedleStore.UI.MVC.Utilities;
using X.PagedList;

namespace BeedleStore.UI.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly BeedleStoreContext _context;

        private readonly IWebHostEnvironment _webHostEnvironment;


        public ProductsController(BeedleStoreContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            #region LINQ Notes

            /*
             * Language INtegrated Query (LINQ)
             * - LINQ is a standardized syntax/set of methods that allow us to operate
             * on collections in C# just as we would with a SQL result set. We can
             * filter, group, select, etc...
             * 
             * SYNTAX EXAMPLE:
             * var [filteredCollection] = _context.[Entity].Where([onTheFlyVariable] => [onTheFlyVariable].Property[Condition]).ToListAsync();
             * 
             * - filteredCollection = The resulting collection once you filter out records you do not need
             * - Entity = The specific database table you are retrieving results from
             * - onTheFlyVariable = Created to represent a single Entity from the table
             * - Property = The property from the Entity to evaluated in the condition
             * - Condition = Your filter criteria
             * 
             */

            #endregion

            //_context.Products > Returns a *COLLECTION* of all products from the table
            //(like a SELECT * FROM [Products])

            //Original code scaffolded by EF:
            //var beedleStoreContext = _context.Products.Include(p => p.Category).Include(p => p.Supplier);

            //Use LINQ to filter this so we only see 'active' products (products that are NOT discontinued)
            var products = _context.Products.Where(p => !p.IsDiscontinued)//SELECT * FROM Products WHERE IsDiscontinued != true
                .Include(p => p.Category) //Similar to a JOIN - gives access to properties from Category
                .Include(p => p.Supplier) // same as above - like a join, gives access to Supplier properties
                .Include(p => p.OrderProducts); //Gives acces to OrderProducts properties

            return View(await products.ToListAsync());
        }

        #region Filter/Paging Steps
        //---- SEARCH ----//
        //1) Create form in the view (for the SEARCH portion, only need 1 textbox and a submit button - <select> will be added later)
        //2) Update controller Action ([A] add param, [B]add search filter logic)

        //---- DDL ----//
        //1) Create ViewData[] object in Controller action (this sends DDL list to the View)
        //2) Add <select> inside of <form>
        //3) Update Controller Action ([A] add param, [B] add category filter logic)

        //---- PAGED LIST ----//
        //1) Install package for X.PagedList.Mvc.Core
        //      - Open Package Manger Console -> select the UI project -> install-package x.pagedlist.mvc.core
        //2) Add using statements and update model declaration in the View
        //3) Add param to Controller Action
        //4) Add page size variable in Action
        //5) Update return statement in Controller Action
        //6) Add Counter in View

        // 7) Create a new CSS file in wwwroot/css named 'PagedList.css'
        //      - NOTE: may need to go to the package's NuGet page and copy the CSS directly OR copy from an existing project :)
        //      - X.PagedList CSS file link (go here to copy the code): https://github.com/dncuug/X.PagedList/blob/master/examples/X.PagedList.Mvc.Example.Core/wwwroot/css/PagedList.css
        // 8) Add a <link> to the _Layout referencing 'PagedList.css'
        #endregion

        //Created a Separate action that returns the same results as Index, but in the View
        // we will use a tiled layout instead of a table

        //Search Step - 2.A
        [AllowAnonymous]
        public async Task<IActionResult> TiledProducts(string searchTerm, int categoryId = 0, int page = 1)
        {
            // paged List - Step 4
            int pageSize = 6;// our tiled view displays rows of 3 products, so this will indicate how many total products to show on a page

            var products = _context.Products.Where(p => !p.IsDiscontinued)
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .Include(p => p.OrderProducts).ToList();

            //DDL - Step 1
            //Note: we copied this line from the existing functionality in Products.Create()
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");

            //DDL - Step 3
            //Add logic to filter the results by categoryId
            #region Optional Category filter
            if (categoryId != 0)
            {
                products = products.Where(p => p.CategoryId == categoryId).ToList();
                //Recreate the dropdown list so the current category is still selected
                ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", categoryId);
            }
            #endregion

            //Search Step - 2.B
            #region Optional Search Filter
            if (!String.IsNullOrEmpty(searchTerm))
            {
                products = products.Where(p =>
                                    p.ProductName.ToLower().Contains(searchTerm.ToLower())
                                    || p.Supplier.SupplierName.ToLower().Contains(searchTerm.ToLower())
                                    || p.Category.CategoryName.ToLower().Contains(searchTerm.ToLower())
                                    || p.ProductDescription.ToLower().Contains(searchTerm.ToLower())).ToList();

                ViewBag.SearchTerm = searchTerm;
                ViewBag.NbrResults = products.Count;
            }
            else
            {
                ViewBag.SearchTerm = null;
                ViewBag.NbrResults = null;
            }
            #endregion


            return View(products.ToPagedList(page, pageSize));
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPrice,ProductDescription,IsDiscontinued,AmountSold,UnitsInStock,UnitsOnOrder,DateReceived,SupplierId,CategoryId,ProductImage")] Product product)
        {
            if (ModelState.IsValid)
            {

                #region File Upload - Create

                //check to see if a file was uploaded
                if(product.Image != null)
                {
                    //check the file type
                    // - Retrieve the extension of the uploaded file
                    string ext = Path.GetExtension(product.Image.FileName);

                    //Create a list of valid extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", ".gif" };

                    //Verify the uploaded files has an extension matching one of
                    // the extensions in the list above. Additionally, let's verify
                    // the file size will work in our .NET app.
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    //The underscores in the number above don't change the number, they
                    // just make it easier to read. 
                    {
                        //Generate a unique filename
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Save the file to the web server (here, saving to wwwwroot/images)
                        //To access the wwwwroot, add a property to the controller for the 
                        // _webHostEnvironment (see the top of this class for our example)
                        string webRootPath = _webHostEnvironment.WebRootPath;

                        //Variable for the full image path (this is where we will save the image)
                        string fullImagePath = webRootPath + "/img/";

                        //Create a MemoryStream to read the image into the server memory
                        using (var memoryStream = new MemoryStream())
                        {
                            //transfer the file from the request to the server memory
                            await product.Image.CopyToAsync(memoryStream);

                            using (var img = Image.FromStream(memoryStream))
                            {
                                //Send the image to the IMageUtilty for resizing and
                                // thumbnail creation.
                                //Items needed for the ImageUtility.ResizeImage()
                                // 1) (int) maximum image size
                                // 2) (int) maximum thumbnail image size
                                // 3) (string) full path where the file should be saved
                                // 4) (Image) the image to save
                                // 5) (string) file name

                                int maxImageSize = 500; //the size in pixels
                                int maxThumbSize = 100;

                                ImageUtility.ResizeImage(fullImagePath, product.ProductImage, img, maxImageSize, maxThumbSize);

                                //myFile.Save("path/to/folder", "filename") > How to save something
                                // that is NOT an image.
                            }
                        }
                    }
                }
                else
                {
                    //If no image was uploaded, assign a default filename
                    //We will also need to download a default image and name
                    // it 'noimage.png' then copy it to the /img folder.
                    product.ProductImage = "noimage.png";
                }

                #endregion
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPrice,ProductDescription,IsDiscontinued,AmountSold,UnitsInStock,UnitsOnOrder,DateReceived,SupplierId,CategoryId,ProductImage")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                #region File Upload - Edit

                //Retain the old image file name so we can delete if a new file is uploaded
                string oldFileName = product.ProductImage;

                //check if the user uploaded a file
                if(product.Image != null)
                {
                    //Get the file's extension
                    string ext = Path.GetExtension(product.Image.FileName);

                    //List the valid extensions
                    string[] validExts = { ".jpeg", ".jpg", ".png", "gif" };

                    //Check the file extension to ensure it is valid AND check the file size
                    if (validExts.Contains(ext.ToLower()) && product.Image.Length < 4_194_303)
                    {
                        //Generate a unique file name
                        product.ProductImage = Guid.NewGuid() + ext;

                        //Build our file path to save the image
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string fullPath = webRootPath + "/img/";

                        //Delete the old image
                        if(oldFileName != "noimage.png")
                        {
                            ImageUtility.Delete(fullPath, oldFileName);
                        }

                        //Save the new image to the WebRoot
                        using (var memoryStream = new MemoryStream())
                        {
                            await product.Image.CopyToAsync(memoryStream);
                            using (var img = Image.FromStream(memoryStream))
                            {
                                int maxImageSize = 500;
                                int maxThumbSize = 100;
                                ImageUtility.ResizeImage(fullPath, product.ProductImage, img, maxImageSize, maxThumbSize);
                            }
                        }
                    }
                }

                #endregion

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "SupplierName", product.SupplierId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'BeedleStoreContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
