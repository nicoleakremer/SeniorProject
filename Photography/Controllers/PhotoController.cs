using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Photography;
using System.IO;


namespace Photography.Controllers
{
    public class PhotoController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /Photo/
        public async Task<ActionResult> Index()
        {
            var photos = db.PHOTOS;
            return View(await photos.ToListAsync());
        }

        public ActionResult AddPhotoToCart(int? id)
        {
            int cust = 0;
            cust = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First().CustomerId;
            CART cart = db.CARTs.Where(x => x.CustomerId == cust).First();
            if (db.AddPhotoToCart(cart.CartId, id, 1) == -1)
            {
                TempData["NoStock"] = "<div class=\"alert alert-warning alert-dismissible\" role=\"alert\"><button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">&times;</span></button><strong>Warning!</strong> This book is out of stock!</div>";
                return RedirectToAction("Index", "Photos");
            }
            else
            {
                return RedirectToAction("Index", "PhotoCart", null);
            }
        }
  
        // GET: /Photo/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.PHOTOS.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // GET: /Photo/Create
       [Authorize(Roles = "v")]
        public ActionResult Create()
        {
            
           
            return View();
        }

        // POST: /Photo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(FormCollection fc, HttpPostedFileBase file, [Bind(Include="PhotoId,InventoryId,VendorId,Title,FirstName,LastName,Genre,Description,Price")] Photo photo)
        {
            //string uploadFolder = Request.PhysicalApplicationPath + "Photos\\";
           // file.SaveAs(uploadFolder + g + ext);
            Photo tbl = new Photo();
            Guid g = Guid.NewGuid();
            var allowedExtensions = new[] {  
            ".JPG", ".png", ".jpg", "jpeg"  
        };
          
          var fileName = Path.GetFileName(file.FileName); //getting only file name(ex-ganesh.jpg)  
            var ext = Path.GetExtension(file.FileName); //getting the extension(ex-.jpg)  
            //file.SaveAs(uploadFolder + g + ext);
           if (allowedExtensions.Contains(ext)) //check what type of extension  
            {
               string name = Path.GetFileNameWithoutExtension(fileName); //getting file name without extension  
                string myfile =  g + ext; //appending the name with id 
                
               // store the file inside ~/project folder(Img)  
               var path = Path.Combine(Server.MapPath("~/Photos"), myfile);
              file.SaveAs(path);
              photo.Path = myfile;
           }
          else
          {
                ViewBag.message = "Please choose only Image file";
          }
        
            if (ModelState.IsValid)
            {
                db.PHOTOS.Add(photo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

           
           
            return View(photo);
        }

      
        // GET: /Photo/Edit/5
         [Authorize(Roles = "a")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.PHOTOS.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
           
           
            return View(photo);
        }

        // POST: /Photo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="PhotoId,InventoryId,VendorId,Title,FirstName,LastName,Genre,Description,Price")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
           
           
            return View(photo);
        }

        // GET: /Photo/Delete/5
        [Authorize(Roles = "a")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = await db.PHOTOS.FindAsync(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: /Photo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Photo photo = await db.PHOTOS.FindAsync(id);
            db.PHOTOS.Remove(photo);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
