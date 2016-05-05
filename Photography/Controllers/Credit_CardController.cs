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
using System.Web.Helpers;
using System.Data.Entity.Validation;
using System.Diagnostics;

namespace Photography.Controllers
{
    public class Credit_CardController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        // GET: /Credit_Card/
        public async Task<ActionResult> Index()
        {
            return View(await db.CREDIT_CARD.ToListAsync());
        }

        // GET: /Credit_Card/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CREDIT_CARD credit_card = await db.CREDIT_CARD.FindAsync(id);
            if (credit_card == null)
            {
                return HttpNotFound();
            }
            return View(credit_card);
        }

        // GET: /Credit_Card/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Credit_Card/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="CardId,CardType,CardNumber,NameOnCard,ExpirationDate,SecurityCode,Salt")] CREDIT_CARD cc)
        {
            if (ModelState.IsValid)
            {

                CUSTOMER c = db.CUSTOMERs.Where(x => x.Email == User.Identity.Name).First();
                cc.CustomerId = c.CustomerId;
                string salt = Crypto.GenerateSalt();
                string hash = Crypto.Hash(salt + cc.CardNumber);
                cc.CardNumber = hash;
                cc.Salt = salt;
                try
                {
                    db.CREDIT_CARD.Add(cc);
                     
                    await db.SaveChangesAsync();
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var validationErrors in e.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string error = string.Format("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage);
                            Debug.WriteLine(error);
                        }
                    }
                }
                return RedirectToAction("Create","Billing");
   
            }

            return View(cc);
        }

        // GET: /Credit_Card/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CREDIT_CARD credit_card = await db.CREDIT_CARD.FindAsync(id);
            if (credit_card == null)
            {
                return HttpNotFound();
            }
            return View(credit_card);
        }

        // POST: /Credit_Card/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="CardId,CardType,CardNumber,NameOnCard,ExpirationDate,SecurityCode")] CREDIT_CARD credit_card)
        {
            if (ModelState.IsValid)
            {
                db.Entry(credit_card).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(credit_card);
        }

        // GET: /Credit_Card/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CREDIT_CARD credit_card = await db.CREDIT_CARD.FindAsync(id);
            if (credit_card == null)
            {
                return HttpNotFound();
            }
            return View(credit_card);
        }

        // POST: /Credit_Card/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CREDIT_CARD credit_card = await db.CREDIT_CARD.FindAsync(id);
            db.CREDIT_CARD.Remove(credit_card);
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
