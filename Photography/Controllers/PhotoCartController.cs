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

namespace Photography.Controllers
{
    public class PhotoCartController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /PhotoCart/
        public async Task<ActionResult> Index()
        {
            CUSTOMER cust = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();
            //var photo_cart = db.PHOTO_CART.Include(p => p.CART).Include(p => p.Photo);
            CART cart = db.CARTs.Where(x => x.CustomerId == cust.CustomerId).First();
            var photo_cart = db.PHOTO_CART.Where(x => x.CartId == cart.CartId);
            return View(await photo_cart.ToListAsync());
        }

        public ActionResult AddToCart(int? id)
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

        // GET: /PhotoCart/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO_CART photo_cart = await db.PHOTO_CART.FindAsync(id);
            if (photo_cart == null)
            {
                return HttpNotFound();
            }
            return View(photo_cart);
        }

        // GET: /PhotoCart/Create
        public ActionResult Create()
        {
            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId");
            ViewBag.PhotoId = new SelectList(db.PHOTOS, "PhotoId", "Title");
            return View();
        }

        // POST: /PhotoCart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="PhotoId,CartId,Quantity,Date")] PHOTO_CART photo_cart)
        {
            if (ModelState.IsValid)
            {
                db.PHOTO_CART.Add(photo_cart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId", photo_cart.CartId);
            ViewBag.PhotoId = new SelectList(db.PHOTOS, "PhotoId", "Title", photo_cart.PhotoId);
            return View(photo_cart);
        }

      

        // GET: /PhotoCart/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PHOTO_CART photo_cart = await db.PHOTO_CART.FindAsync(id);
            if (photo_cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId", photo_cart.CartId);
            ViewBag.PhotoId = new SelectList(db.PHOTOS, "PhotoId", "Title", photo_cart.PhotoId);
            return View(photo_cart);
        }

        // POST: /PhotoCart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="PhotoId,CartId,Quantity,Date")] PHOTO_CART photo_cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(photo_cart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId", photo_cart.CartId);
            ViewBag.PhotoId = new SelectList(db.PHOTOS, "PhotoId", "Title", photo_cart.PhotoId);
            return View(photo_cart);
        }

        // GET: /PhotoCart/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            CUSTOMER cust = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();
            CART cart = db.CARTs.Where(x => x.CustomerId == cust.CustomerId).First();
            Photo photo = await db.PHOTOS.FindAsync(id);
            PHOTO_CART photo_cart = db.PHOTO_CART.Where(x => x.CartId == cart.CartId).First();

            if (photo_cart == null)
            {
                return HttpNotFound();
            }

            db.removeFromCart(photo.PhotoId, photo_cart.CartId);
            
            return RedirectToAction("Index");
        }

        // POST: /PhotoCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PHOTO_CART photo_cart = await db.PHOTO_CART.FindAsync(id);
            db.PHOTO_CART.Remove(photo_cart);
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
