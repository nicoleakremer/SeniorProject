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
using System.Security.Cryptography;
using System.Web.Helpers;


namespace Photography.Controllers
{
    public class RegisterController : Controller
    {
        private dataModel db = new dataModel();

        // GET: /Register/
        public async Task<ActionResult> Index()
        {
            return View(await db.CUSTOMERs.ToListAsync());
        }

        // GET: /Register/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUSTOMER customer = await db.CUSTOMERs.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: /Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Register/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="CustomerId,Email,Password,FirstName,LastName,Phone")] CUSTOMER customer)
        {
          
            if (ModelState.IsValid)
            {
             /*
                string salt = Crypto.GenerateSalt();
                string passAndSalt;
                string UserHash;
                passAndSalt = customer.Password + salt;
                UserHash = Crypto.Hash(passAndSalt);
                */
                db.CUSTOMERs.Add(customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index","Home");
            }

            return View(customer);
        }

        // GET: /Register/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUSTOMER customer = await db.CUSTOMERs.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Register/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="CustomerId,Email,Password,FirstName,LastName,Phone")] CUSTOMER customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: /Register/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CUSTOMER customer = await db.CUSTOMERs.FindAsync(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: /Register/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            CUSTOMER customer = await db.CUSTOMERs.FindAsync(id);
            db.CUSTOMERs.Remove(customer);
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
