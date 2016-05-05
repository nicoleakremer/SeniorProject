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
using System.Data.Entity.Core.Objects;
using System.Web.Security;


namespace Photography.Controllers
{
    public class RegisterController : Controller
    {
        private naKremerEntities db = new naKremerEntities();

        
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(CUSTOMER c)
        {

            ObjectResult<string> password = db.findPassword(c.Email);
            ObjectResult<string> salt = db.findSalt(c.Email);
            string actPass = password.First();
            string actSalt = salt.First();
            string hash = Crypto.Hash(actSalt + c.Password);
            if (actPass == hash)
            {
                //return and login
                FormsAuthentication.SetAuthCookie(c.Email, false);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewBag.Msg = "Invalid User";
                   return View();
                //return error message/page
            }
            return View();
        }

        public async Task<ActionResult> Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="CustomerId,Email,Password,FirstName,LastName,Phone")] CUSTOMER c)
        {
          
            if (ModelState.IsValid)
            {
                string salt = Crypto.GenerateSalt();
                string hash = Crypto.Hash(salt + c.Password);
                c.Password = hash;
                c.Salt = salt;
                c.Roles = "u";
                db.CUSTOMERs.Add(c);
                await db.SaveChangesAsync();

                db.AddCart(c.Email);
                FormsAuthentication.SetAuthCookie(c.Email, false);
                return RedirectToAction("Index","Home");
            }
       
        return RedirectToAction("Index", "Home");
        }

        public ActionResult CreateVendor()
        {
            return View();
        }
        [Authorize(Roles = "v")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVendor([Bind(Include = "CustomerId,Email,Password,FirstName,LastName,Phone")] CUSTOMER c)
        {

            if (ModelState.IsValid)
            {
                string salt = Crypto.GenerateSalt();
                string hash = Crypto.Hash(salt + c.Password);
                c.Password = hash;
                c.Salt = salt;
                c.Roles = "v";
                db.CUSTOMERs.Add(c);
                await db.SaveChangesAsync();
                return RedirectToAction("Create", "Credit_Card");
            }

            return RedirectToAction("Index", "Home");
        }
         [Authorize(Roles = "a")]
        public ActionResult CreateAdmin()
        {
            return View();
        }
        [Authorize(Roles="a")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAdmin([Bind(Include = "CustomerId,Email,Password,FirstName,LastName,Phone")] CUSTOMER c)
        {

            if (ModelState.IsValid)
            {
                string salt = Crypto.GenerateSalt();
                string hash = Crypto.Hash(salt + c.Password);
                c.Password = hash;
                c.Salt = salt;
                c.Roles = "a";
                db.CUSTOMERs.Add(c);
                await db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
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
