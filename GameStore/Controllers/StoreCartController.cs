using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class StoreCartController : Controller
    {
        // GET: StoreCart
        public ActionResult StoreCart()
        {
            return View();
        }
    }
}