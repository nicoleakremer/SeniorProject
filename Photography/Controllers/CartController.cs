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
    public class CartController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /Cart/
        public async Task<ActionResult> Index()
        {
     
            CUSTOMER customer = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();
            CART cart = db.CARTs.Where(x => x.CustomerId == customer.CustomerId).First();
            
            var list = db.PHOTO_CART.Where(x => x.CartId == cart.CartId);
            List<Photo> CartList = new List<Photo>();
            foreach (var item in list)
            {

                Photo Photo = db.PHOTOS.Where(x => x.PhotoId == item.PhotoId).First();

                CartList.Add(Photo);

            }
            return View(CartList);
        
        }


        [HttpPost]
            public ActionResult AddPhotoToCart(int PhotoId)
        {
           // db.AddPhotoToCart(PhotoId,User.Identity.GetEmail());
            return RedirectToAction("Index", "PhotoCart");
        }

        [HttpPost]
        public ActionResult RemovePhotoFromCart(int PhotoId)
        {
           // db.RemovePhotoFromCart(User.Identity.Name(),PhotoId);
            return RedirectToAction("Index", "PhotoCart");
        }

        // GET: /Cart/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CART cart = await db.CARTs.FindAsync(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: /Cart/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email");
          
            return View();
        }

        // POST: /Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="CartId,CustomerId,VendorId,IsActive")] CART cart)
        {
            if (ModelState.IsValid)
            {
                db.CARTs.Add(cart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", cart.CustomerId);
           
            return View(cart);
        }

        // GET: /Cart/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CART cart = await db.CARTs.FindAsync(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", cart.CustomerId);
           
            return View(cart);
        }

        // POST: /Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="CartId,CustomerId,VendorId,IsActive")] CART cart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", cart.CustomerId);
           
            return View(cart);
        }

        // GET: /Cart/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CART cart = await db.CARTs.FindAsync(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: /Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CART cart = await db.CARTs.FindAsync(id);
            db.CARTs.Remove(cart);
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
