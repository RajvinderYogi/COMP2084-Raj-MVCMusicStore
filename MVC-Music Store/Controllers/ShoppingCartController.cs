using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_Music_Store.Models;

namespace MVC_Music_Store.Controllers
{
    public class ShoppingCartController : Controller
    {
        //DB connection
        MusicStoreModel db = new MusicStoreModel();
        // GET: ShoppingCart
        public ActionResult Index()
        {
            //get current cart
            var cart = ShoppingCart.GetCart(this.HttpContext);

            //set up ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };

            //pass cart to the index view
            return View(viewModel);
        }

        // GET: AddToCart/300
        public ActionResult AddToCart(int Id)
        {
            //get current cart if any
            var cart = ShoppingCart.GetCart(this.HttpContext);

            //add the current Album to cart
            cart.AddToCart(Id);

            //redirect to index which will display the current cart
            return RedirectToAction("Index");
        }
        [Authorize]
        //Get Checkout
        public ActionResult Checkout()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        //Post Checkout
        public ActionResult Checkout(FormCollection values)
        {
            // create a new order and populate the fields from the form
            var order = new Order();
            TryUpdateModel(order);

            //check the PromoCode for a value FREE
            if(values["PromoCode"]!="FREE")
            {
                ViewBag.Message = "Use Promo Code FREE";
                return View(order);
            }
            else
            {
                //promo code is good so save the order
                //auto-fill the username, email, date and total
                order.Username = User.Identity.Name;
                order.Email = User.Identity.Name;
                order.OrderDate = DateTime.Now;

                var cart = ShoppingCart.GetCart(this.HttpContext);
                order.Total = cart.GetTotal();

                //save
                db.Orders.Add(order);
                db.SaveChanges();

                // save order details from cart
                var cartItems = cart.GetCartItems();

                foreach(Cart item in cartItems)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.OrderId = order.OrderId;
                    orderDetail.AlbumId = item.AlbumId;
                    orderDetail.Quantity = item.Count;
                    orderDetail.UnitPrice = item.Album.Price;
                    db.OrderDetails.Add(orderDetail);
                }
                db.SaveChanges();

                //empty the user's cart
                cart.EmptyCart();
            }
            //redirect the order Details
            return RedirectToAction("Details", "Orders", new { id = order.OrderId });
        }
    }
}