using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using chatting.Models;

namespace chatting.Controllers
{
    public class ContentController : Controller
    {
        private chattingonlyEntities1 db = new chattingonlyEntities1();

        [Authorize]
        public ActionResult AddContent()
        {

            content newContent = new content();

            return View(newContent);
        }
        public ActionResult Details(int id)
        {
            content currentcontent = db.content.FirstOrDefault(c => c.cid == id);

            if (currentcontent != null)
            {
                return View(currentcontent);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }
        [HttpPost]
        public ActionResult AddContent(content content)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    content.date = DateTime.Now;

                    string userId = User.Identity.Name;
                    content.user = db.user.FirstOrDefault(u => u.useremail == userId);

                    db.content.Add(content);
                    db.SaveChanges();


                    int contentId = content.cid;


                    int contentId = content.cid;

                    if (Request.Files.Count > 0)
                    {
                        HttpPostedFileBase imageFile = Request.Files[0];
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                            string extension = Path.GetExtension(imageFile.FileName);

                            if (allowedExtensions.Contains(extension.ToLower()))
                            {

                                byte[] imageData;
                                using (var binaryReader = new BinaryReader(imageFile.InputStream))
                                {
                                    imageData = binaryReader.ReadBytes(imageFile.ContentLength);
                                }


                                var picture = new picture
                                {
                                    contentid = contentId,
                                    picture1 = imageData
                                };


                                db.picture.Add(picture);
                                db.SaveChanges();
                            }
                        }
                    }

                    return RedirectToAction("Index", "Home");
                }

                return View(content);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "İçerik eklenirken bir hata oluştu: " + ex.Message;
                return View(content);
            }
        }







    }
}