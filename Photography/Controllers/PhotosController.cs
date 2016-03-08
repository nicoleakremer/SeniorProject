using Photography.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Photography.Controllers
{
    public class PhotosController : Controller
    {
        private dataModel db = new dataModel();
        // GET: /Photos/
        public ActionResult Index()
        {
            var photo = db.PHOTOS;
            return View(photo.ToList());
        }
	}
}