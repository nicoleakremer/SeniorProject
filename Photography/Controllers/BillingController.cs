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
    public class BillingController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /Billing/
        public async Task<ActionResult> Index()
        {
            var billings = db.BILLINGs.Include(b => b.CREDIT_CARD);
            return View(await billings.ToListAsync());
        }

        // GET: /Billing/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILLING billing = await db.BILLINGs.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // GET: /Billing/Create
        public ActionResult Create()
        {
            ViewBag.CardId = new SelectList(db.CREDIT_CARD, "CardId", "CardType");
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email");
          
            return View();
        }

        // POST: /Billing/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="BillingId,CustomerId,CardId,State,Street,City,Zip")] BILLING billing)
        {
            if (ModelState.IsValid)
            {
                CUSTOMER customer = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();
                billing.CustomerId = customer.CustomerId;
                billing.CardId = db.CREDIT_CARD.Where(x => x.CustomerId == customer.CustomerId).First().CardId;
                

                db.BILLINGs.Add(billing);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Invoice");
            }

            ViewBag.CardId = new SelectList(db.CREDIT_CARD, "CardId", "CardType", billing.CardId);
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", billing.CustomerId);
         
            return View(billing);
        }

        // GET: /Billing/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILLING billing = await db.BILLINGs.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            ViewBag.CardId = new SelectList(db.CREDIT_CARD, "CardId", "CardType", billing.CardId);
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", billing.CustomerId);
           
            return View(billing);
        }

        // POST: /Billing/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="BillingId,CustomerId,VendorId,CardId,State,Street,City,Zip")] BILLING billing)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billing).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CardId = new SelectList(db.CREDIT_CARD, "CardId", "CardType", billing.CardId);
            ViewBag.CustomerId = new SelectList(db.CUSTOMERs, "CustomerId", "Email", billing.CustomerId);
           
            return View(billing);
        }

        // GET: /Billing/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BILLING billing = await db.BILLINGs.FindAsync(id);
            if (billing == null)
            {
                return HttpNotFound();
            }
            return View(billing);
        }

        // POST: /Billing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BILLING billing = await db.BILLINGs.FindAsync(id);
            db.BILLINGs.Remove(billing);
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
