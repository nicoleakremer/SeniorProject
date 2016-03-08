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
    public class PhotoController : Controller
    {
        private DataModel db = new DataModel();

        // GET: /Photo/
        public ActionResult Index()
        {
            var photo = db.PHOTOS.Include(p => p.INVENTORY).Include(p => p.VENDOR);
            return View(photo.ToList());
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
        public ActionResult Create()
        {
            ViewBag.InventoryId = new SelectList(db.INVENTORies, "InventoryId", "InventoryId");
            ViewBag.VendorId = new SelectList(db.VENDORs, "VendorId", "Email");
            return View();
        }

        // POST: /Photo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="PhotoId,InventoryId,VendorId,Title,FirstName,LastName,Genre,Description,Price")] Photo photo)
        {
            if (ModelState.IsValid)
            {
                db.PHOTOS.Add(photo);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.InventoryId = new SelectList(db.INVENTORies, "InventoryId", "InventoryId", photo.InventoryId);
            ViewBag.VendorId = new SelectList(db.VENDORs, "VendorId", "Email", photo.VendorId);
            return View(photo);
        }

        // GET: /Photo/Edit/5
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
            ViewBag.InventoryId = new SelectList(db.INVENTORies, "InventoryId", "InventoryId", photo.InventoryId);
            ViewBag.VendorId = new SelectList(db.VENDORs, "VendorId", "Email", photo.VendorId);
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
            ViewBag.InventoryId = new SelectList(db.INVENTORies, "InventoryId", "InventoryId", photo.InventoryId);
            ViewBag.VendorId = new SelectList(db.VENDORs, "VendorId", "Email", photo.VendorId);
            return View(photo);
        }

        // GET: /Photo/Delete/5
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
