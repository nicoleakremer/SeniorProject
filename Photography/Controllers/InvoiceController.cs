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
    public class InvoiceController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /Invoice/
        public async Task<ActionResult> Index()
        {
            CUSTOMER customer = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();


            var invoices = db.INVOICEs.Include(i => i.CART).Include(i => i.CUSTOMER);
            return View(await invoices.ToListAsync());
        }

        // GET: /Invoice/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INVOICE invoice = await db.INVOICEs.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: /Invoice/Create
        public ActionResult Create()
        {
            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId");
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email");
            return View();
        }

        // POST: /Invoice/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="InvoiceId,CustomerId,CartId,TotalCost,DateOrder")] INVOICE invoice)
        {
            if (ModelState.IsValid)
            {
                db.INVOICEs.Add(invoice);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId", invoice.CartId);
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", invoice.CustomerId);
            return View(invoice);
        }

        // GET: /Invoice/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INVOICE invoice = await db.INVOICEs.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId", invoice.CartId);
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", invoice.CustomerId);
            return View(invoice);
        }

        // POST: /Invoice/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="InvoiceId,CustomerId,CartId,TotalCost,DateOrder")] INVOICE invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CartId = new SelectList(db.CARTs, "CartId", "CartId", invoice.CartId);
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", invoice.CustomerId);
            return View(invoice);
        }

        // GET: /Invoice/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            INVOICE invoice = await db.INVOICEs.FindAsync(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: /Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            INVOICE invoice = await db.INVOICEs.FindAsync(id);
            db.INVOICEs.Remove(invoice);
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
