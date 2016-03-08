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
    public class VendorController : Controller
    {
        private dataModel db = new dataModel();

        // GET: /Vendor/
        public async Task<ActionResult> Index()
        {
            return View(await db.VENDORs.ToListAsync());
        }

        // GET: /Vendor/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDOR vendor = await db.VENDORs.FindAsync(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // GET: /Vendor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Vendor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="VendorId,Email,Password,FirstName,LastName,Phone")] VENDOR vendor)
        {
            if (ModelState.IsValid)
            {
                db.VENDORs.Add(vendor);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(vendor);
        }

        // GET: /Vendor/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDOR vendor = await db.VENDORs.FindAsync(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: /Vendor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="VendorId,Email,Password,FirstName,LastName,Phone")] VENDOR vendor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendor).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(vendor);
        }

        // GET: /Vendor/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VENDOR vendor = await db.VENDORs.FindAsync(id);
            if (vendor == null)
            {
                return HttpNotFound();
            }
            return View(vendor);
        }

        // POST: /Vendor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            VENDOR vendor = await db.VENDORs.FindAsync(id);
            db.VENDORs.Remove(vendor);
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
