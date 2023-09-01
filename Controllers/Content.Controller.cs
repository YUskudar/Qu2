using Qu2SM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qu2SM.Controllers
{
    public class ContentController : Controller
    {

        private chattingonlyEntities db = new chattingonlyEntities();

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
           

        //Burada Düz metin olarak content, resim olarak ve video olarak content yaratma aksiyonunu yazdım.
            [HttpPost]
            public ActionResult AddContent(content content, HttpPostedFileBase imageFile, HttpPostedFileBase videoFile)
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

                        // Alt tarafta kullanıcının yüklemiş olduğu resmi byte dizisine dönüştürüp sisteme kaydediyorum aynı işlemi video kısmında da yaptım
                        if (imageFile != null && imageFile.ContentLength > 0)
                        {
                            string[] imageExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
                            string imageExtension = Path.GetExtension(imageFile.FileName);

                            if (imageExtensions.Contains(imageExtension.ToLower()))
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

                        // Video yükleme
                        if (videoFile != null && videoFile.ContentLength > 0)
                        {
                            string[] videoExtensions = { ".mp4", ".avi", ".mov" };
                            string videoExtension = Path.GetExtension(videoFile.FileName);

                            if (videoExtensions.Contains(videoExtension.ToLower()))
                            {
                                byte[] videoData;
                                using (var binaryReader = new BinaryReader(videoFile.InputStream))
                                {
                                    videoData = binaryReader.ReadBytes(videoFile.ContentLength);
                                }

                                var video = new video
                                {
                                    content_id = contentId,
                                    video1 = videoData
                                };

                                db.video.Add(video);
                                db.SaveChanges();
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
