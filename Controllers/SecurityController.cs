using chatting.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace chatting.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        chattingonlyEntities db = new chattingonlyEntities ();

        public ActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public ActionResult Login(user user)
        {
            var userInDb = db.user.FirstOrDefault(x => x.useremail == user.useremail && x.password == user.password);

            if (userInDb != null)
            {

                FormsAuthentication.SetAuthCookie(user.useremail, false);


                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Mesaj = "Geçersiz kullanıcı adı veya parola.";
                return View();
            }
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(user user, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                var existingUser = db.user.FirstOrDefault(x => x.useremail == user.useremail);

                if (existingUser != null)
                {
                    ViewBag.ErrorMessage = "Bu kullanıcı adı zaten kullanılıyor.";
                    return View();
                }

                db.user.Add(user);
                db.SaveChangesAsync();
                return RedirectToAction("Login");
            }

            return View(user);
        }











        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Security");
        }
    }
}