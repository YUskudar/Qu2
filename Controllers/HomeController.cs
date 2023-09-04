using Qu2SM.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Qu2SM.Controllers
{
    public class HomeController : Controller
    {
        private chattingonlyEntities db = new chattingonlyEntities();
        public ActionResult Index()
        {
            string userName = User.Identity.Name;
            ViewBag.UserName = userName;

            List<content> allContents = db.content.ToList();

            return View(allContents);
        }
        public ActionResult UserDetails(int id) //Contenti Atan kullanıcının detaylarını görebilir.
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
        public ActionResult AddComment()
        {

            comment newComment = new comment();

            return View(newComment);
        }
        [HttpPost]
        public ActionResult AddComment(comment comment, int? selectedContentId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    comment.date = DateTime.Now;

                    string userId = User.Identity.Name;
                    comment.user = db.user.FirstOrDefault(u => u.useremail == userId);

                    comment.content_id = selectedContentId;

                    db.comment.Add(comment);
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Yorum eklerken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public ActionResult AddContent(content content, HttpPostedFileBase imageFile, HttpPostedFileBase videoFile) //Bu kısım aslında partial view olarak tasarlandı kullanıc anasayfada da içerik üretebilicek.
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
        

        [HttpPost]
        public ActionResult DeleteContent(int contentId) //Bu kısımda Kullanıcı kendi profiline girip kendisinin oluşturduğu içerikleri silebilir
        {
            var content = db.content.FirstOrDefault(c => c.cid == contentId);

            if (content != null)
            {
                if (content.picture != null)
                {
                    foreach (var picture in content.picture.ToList())
                    {
                        db.picture.Remove(picture);
                    }
                }

                if (content.video != null)
                {
                    foreach (var video in content.video.ToList())
                    {
                        db.video.Remove(video);
                    }
                }

                if (content.likes != null)
                {
                    foreach (var like in content.likes.ToList())
                    {
                        db.likes.Remove(like);
                    }
                }

                if (content.comment != null)
                {
                    foreach (var comment in content.comment.ToList())
                    {
                        db.comment.Remove(comment);
                    }
                }

                db.content.Remove(content);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "User");
        }
        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            try
            {
                var comment = db.comment.FirstOrDefault(c => c.coid == commentId);

                if (comment == null)
                {
                    return RedirectToAction("NotFound", "Error");
                }

                string userId = User.Identity.Name;

                if (comment.user.useremail == userId || comment.content.user.useremail == userId)
                {
                    db.comment.Remove(comment);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                else
                {
                    ViewBag.ErrorMessage = "Bu yorumu silme izniniz yok.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Yorum silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

    }
}