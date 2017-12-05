using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_Music_Store.Models
{
    public class ShoppingCart
    {
        //DB connection
        MusicStoreModel db = new MusicStoreModel();

        //string for unique cart ID
        string ShoppingCartId { get; set; }

        //get the current cart content
        // include httpcontextbase to access current user info
        public static ShoppingCart GetCart(HttpContextBase context)
        {
            var cart = new ShoppingCart();

            //check for an existing Cart Id
            if (context.Session["CartId"] == null)
            {
                // if we have no current cart in session, check if user is logged in
                if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
                {
                    context.Session["CartId"] = context.User.Identity.Name;
                }
                else
                {
                    //user is anonymus, generate unique CartId
                    Guid tempCartId = Guid.NewGuid();
                    context.Session["CartId"] = tempCartId;
                }
            }
            cart.ShoppingCartId = context.Session["CartId"].ToString();

            return cart;
        }

        //Add to cart 

        public void AddToCart(int Id)
        {
            //is this album already in this cart?
            var cartItem = db.Carts.SingleOrDefault(a => a.AlbumId == Id
            && a.CartId == ShoppingCartId);


            if (cartItem == null)
            {
                cartItem = new Cart
                {
                    AlbumId = Id,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };

                //save to DB
                db.Carts.Add(cartItem);
            }
            else
            {
                cartItem.Count++;
            }
                db.SaveChanges();
            }
        //remove Item from cart
        public void RemoveFromCart(int id)
        {
            //get the selected album
            var item = db.Carts.SingleOrDefault(c => c.AlbumId == id
            && c.CartId == ShoppingCartId);

            if(item != null)
            {
                //if quantity is 1, delete the item
                if(item.Count==1)
                {
                    db.Carts.Remove(item);
                }
                else
                {
                    //if quantity is > 1, substract 1 from the quantity
                    item.Count--;
                }
                db.SaveChanges();
            }
        }

        //get the items in the cart
        public List<Cart> GetCartItems()
        {
            return db.Carts.Where(c => c.CartId == ShoppingCartId).ToList();
        }

        // Get Cart Total
        public decimal GetTotal()
        {
            //get  the albums in the current cart 
            //calculate the total for each (count*price)
            //sum all the line totals together
            decimal? total = (from c in db.Carts
                              where c.CartId == ShoppingCartId
                              select (int?)c.Count * c.Album.Price).Sum();
            return total ?? decimal.Zero;
        }

        //empty cart
        public void EmptyCart()
        {
            //get all the items in the cart table for th curren tuser
            var cartItems = db.Carts.Where(c => c.CartId == ShoppingCartId);

            foreach(Cart item in cartItems)
            {
                db.Carts.Remove(item);
            }
            db.SaveChanges();
        }
    }
}