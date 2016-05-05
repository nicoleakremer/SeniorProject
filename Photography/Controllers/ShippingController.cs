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
    public class ShippingController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /Shipping/
        public async Task<ActionResult> Index()
        {
            var shippings = db.SHIPPINGs.Include(s => s.CUSTOMER);
            return View(await shippings.ToListAsync());
        }

        // GET: /Shipping/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPING shipping = await db.SHIPPINGs.FindAsync(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        // GET: /Shipping/Create
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email");
          
            return View();
        }

        // POST: /Shipping/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ShippingId,CustomerId,VendorId,State,Street,City,Zip")] SHIPPING shipping)
        {
            if (ModelState.IsValid)
            {
                CUSTOMER customer = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();
                shipping.CustomerId = customer.CustomerId;
                
                
                
                db.SHIPPINGs.Add(shipping);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Invoice");
            }

            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", shipping.CustomerId);
          
            return View(shipping);
        }

        // GET: /Shipping/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPING shipping = await db.SHIPPINGs.FindAsync(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", shipping.CustomerId);
            
            return View(shipping);
        }

        // POST: /Shipping/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ShippingId,CustomerId,VendorId,State,Street,City,Zip")] SHIPPING shipping)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shipping).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", shipping.CustomerId);
           
            return View(shipping);
        }

        // GET: /Shipping/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SHIPPING shipping = await db.SHIPPINGs.FindAsync(id);
            if (shipping == null)
            {
                return HttpNotFound();
            }
            return View(shipping);
        }

        // POST: /Shipping/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SHIPPING shipping = await db.SHIPPINGs.FindAsync(id);
            db.SHIPPINGs.Remove(shipping);
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
