using Qu2SM.Models;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using GoogleAuthentication.Services;
using System.Threading.Tasks;

namespace chatting.Controllers
{
    [AllowAnonymous]
    public class SecurityController : Controller
    {
        private chattingonlyEntities db = new chattingonlyEntities();

        
        public ActionResult Login()
        {
            var clientId = "25467907298-qhs74gbcv5u3hve0i9kmfpahpdmeu8sm.apps.googleusercontent.com";
            var url = "https://localhost:44337/Security/GoogleLoginCallback";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.response = response;
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(admin admin)
        {
            var adminInDb = db.admin.FirstOrDefault(a => a.adminemail == admin.adminemail && a.adminpasswd == admin.adminpasswd);

            if (adminInDb != null)
            {
                FormsAuthentication.SetAuthCookie(admin.adminemail, false);

                Session["AdminLoggedIn"] = true;

                return RedirectToAction("Index", "Kategori");
            }
            else
            {
                TempData["ErrorMessage"] = "E-posta veya şifre yanlış.";
                return RedirectToAction("AdminLogin");
            }
        }

        [HttpPost]
        public ActionResult Login(user user)
        {
            var userInDb = db.user.FirstOrDefault(x => x.useremail == user.useremail && x.password == user.password);

            if (userInDb != null)
            {
                FormsAuthentication.SetAuthCookie(user.useremail, false);

                Session["AdminLoggedIn"] = false;

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
                db.SaveChanges();
                return RedirectToAction("Login");
            }

            return View(user);
        }


        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Security");
        }
   
        public async Task<ActionResult> GoogleLoginCallback(string code)
        {
            var clientId = "25467907298-qhs74gbcv5u3hve0i9kmfpahpdmeu8sm.apps.googleusercontent.com";
            var url = "https://localhost:44337/Security/GoogleLoginCallback";
            var clientsecret = "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";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientsecret, url);
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken.ToString());
            FormsAuthentication.SetAuthCookie(userProfile, false);

            return RedirectToAction("Index","Home");
        }

    }

        
}
