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
    }
}