using Qu2SM.Models;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qu2SM.Controllers
{
    public class UserController : Controller
    {
        chattingonlyEntities db = new chattingonlyEntities();

        public ActionResult Index()
        {
            user currentUser = GetCurrentUser();

            if (currentUser != null)
            {
                return View(currentUser);
            }
            else
            {
                return RedirectToAction("Login", "Security");
            }
        }


        public ActionResult Update()
        {
            return View();
        }

        public user GetCurrentUser()
        {
            string currentUserName = User.Identity.Name;

            user currentUser = db.user.FirstOrDefault(u => u.useremail == currentUserName);

            return currentUser;
        }
        

        [HttpPost]

        public ActionResult Index(HttpPostedFileBase imageFile)
        {
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                string extension = Path.GetExtension(imageFile.FileName);

                if (!allowedExtensions.Contains(extension.ToLower()))
                {
                    ViewBag.ErrorMessage = "Geçersiz dosya uzantısı. Lütfen .jpg, .jpeg, .png veya .gif dosyaları yükleyin.";
                    return RedirectToAction("Index");
                }

                user currentUser = GetCurrentUser();

                if (currentUser != null)
                {
                    using (var binaryReader = new BinaryReader(imageFile.InputStream))
                    {
                        currentUser.userpp = binaryReader.ReadBytes(imageFile.ContentLength);
                    }

                    db.Entry(currentUser).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            ViewBag.ErrorMessage = "Lütfen bir dosya seçin.";
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult Update(HttpPostedFileBase imageFile, user updatedUser)//Bu ksıımda kullanıcı kendisi hakkında istediği bilgiyi güncelleyebilir.
            {
            if (User.Identity.IsAuthenticated)
            {
                var currentUser = GetCurrentUser();

                if (currentUser != null)
                { 
                   if (imageFile != null && imageFile.ContentLength > 0)
                    {
                        string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };

                        string extension = Path.GetExtension(imageFile.FileName);

                        if (!allowedExtensions.Contains(extension.ToLower()))
                        {
                           ViewBag.ErrorMessage = "Geçersiz dosya uzantısı. Lütfen .jpg, .jpeg, .png veya .gif dosyaları yükleyin.";
                           return RedirectToAction("Index");
                        }

                        using (var binaryReader = new BinaryReader(imageFile.InputStream))
                        {
                           currentUser.userpp = binaryReader.ReadBytes(imageFile.ContentLength);
                        }
                   }

                        currentUser.useremail = updatedUser.useremail ?? currentUser.useremail;
                        currentUser.usernickname = updatedUser.usernickname ?? currentUser.usernickname;
                        currentUser.username = updatedUser.username ?? currentUser.username;
                        currentUser.usersurname = updatedUser.usersurname ?? currentUser.usersurname;
                        currentUser.userage = updatedUser.userage ?? currentUser.userage;
                        currentUser.usercity = updatedUser.usercity ?? currentUser.usercity;
                        currentUser.userphone = updatedUser.userphone ?? currentUser.userphone;
                        currentUser.userjob = updatedUser.userjob ?? currentUser.userjob;

                        db.SaveChanges();

                        return RedirectToAction("Index");
                    }
                }

                ViewBag.ErrorMessage = "Lütfen oturum açın.";
                return RedirectToAction("Index", "Home");
            }

        
    }

    

}
