using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chatting.Models; 
namespace chatting.Controllers
{
    public class HomeController : Controller
    {
        private chattingonlyEntities1 db = new chattingonlyEntities1(); 
        public ActionResult Index()
        {
            string userName = User.Identity.Name;
            ViewBag.UserName = userName;

            List<content> allContents = db.content.ToList();

            return View(allContents);
        }
        public ActionResult UserDetails(int id)
        {
            var user = db.user.FirstOrDefault(u => u.userid == id);

            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

    }
}
