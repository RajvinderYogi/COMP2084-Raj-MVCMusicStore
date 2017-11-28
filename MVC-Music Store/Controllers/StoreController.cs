using MVC_Music_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Music_Store.Controllers
{
    public class StoreController : Controller
    {
        MusicStoreModel db = new MusicStoreModel();
        // GET: Store
        public ActionResult Index()
        {
            //var genres = new List<Genre>();

            //for(int i=1; i<=10; i++)
            //{
            //    genres.Add(new Genre { Name = "Genre" + i.ToString() });
            //}
            var genres = db.Genres.ToList().OrderBy(g=>g.Name);
            return View(genres);
            //ViewBag.genres = genres;
        }

        // GET: Store/Browse
        public ActionResult Browse(string genre)
        {
            //add selected genre to view bag so we can display it in the browse view

            var g = db.Genres.Include("Albums")
                .SingleOrDefault(gn => gn.Name == genre);
            return View(g);
        }  
       
    }
}
