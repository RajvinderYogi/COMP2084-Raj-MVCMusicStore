using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Music_Store.Models;

namespace MVC_Music_Store.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private MusicStoreModel db = new MusicStoreModel();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Where(o => o.Username == User.Identity.Name);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            //iscurrebt user the owne of this order??
            if(order.Username==User.Identity.Name)
            {
                return View(order);
            }
            else
            {
                return View("Error");
            }
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
