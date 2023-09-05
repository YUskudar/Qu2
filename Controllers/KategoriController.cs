using Qu2SM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qu2SM.Controllers
{

    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["AdminLoggedIn"] != null && (bool)httpContext.Session["AdminLoggedIn"])
            {
                return true; 
            }
            return false;
        }
    }
    [AdminAuthorize]

    public class KategoriController : Controller
    {
        private chattingonlyEntities _context;

        public KategoriController()
        {
            _context = new chattingonlyEntities(); 
        }

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return View();
            }

            return RedirectToAction("Unauthorized", "Error");
        }

        public ActionResult Users()
        {
            List<user> users = _context.user.ToList();
            return View(users);
        }
        public ActionResult DeleteUser(int id)
        {
            user user = _context.user.FirstOrDefault(u => u.userid == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var userContents = _context.content.Where(c => c.user_id == id).ToList();

            foreach (var content in userContents)
            {
                var contentPictures = _context.picture.Where(p => p.contentid == content.cid).ToList();
                _context.picture.RemoveRange(contentPictures);
            }

            _context.content.RemoveRange(userContents);

            _context.comment.RemoveRange(user.comment);
            _context.groupmessage.RemoveRange(user.groupmessage);
            _context.usergroups.RemoveRange(user.usergroups);
            _context.likes.RemoveRange(user.likes);

            _context.user.Remove(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Contents()
        {
            List<content> contents = _context.content.ToList();
            foreach (var content in contents)
            {
                if (_context.picture.Any(p => p.contentid == content.cid))
                {
                }
                else if (_context.video.Any(v => v.content_id == content.cid))
                {
                }
                else
                {
                }
            }
            return View(contents);
        }
        public ActionResult ContentDetail(int id)
        {
            var content = _context.content.FirstOrDefault(c => c.cid == id);

            if (content == null)
            {
                return HttpNotFound();
            }

            return View("ContentDetail", content);
        }
        public ActionResult DeleteContent(int id)
        {
            var content = _context.content.FirstOrDefault(c => c.cid == id);

            if (content == null)
            {
                return HttpNotFound();
            }

            var contentPictures = _context.picture.Where(p => p.contentid == content.cid).ToList();
            _context.picture.RemoveRange(contentPictures);

            var contentVideos = _context.video.Where(v => v.content_id == content.cid).ToList();
            _context.video.RemoveRange(contentVideos);

            var contentComments = _context.comment.Where(c => c.content_id == content.cid).ToList();
            _context.comment.RemoveRange(contentComments);

            var contentLikes = _context.likes.Where(l => l.content_id == content.cid).ToList();
            _context.likes.RemoveRange(contentLikes);

            _context.content.Remove(content);
            _context.SaveChanges();

            return RedirectToAction("Contents");
        }

        public ActionResult Groups()
        {
            List<usergroups> groups = _context.usergroups.ToList();
            return View(groups);
        }

        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            try
            {
                var comment = _context.comment.FirstOrDefault(c => c.coid == commentId);

                

                _context.comment.Remove(comment);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Yorum silinirken bir hata oluştu: " + ex.Message;
                return RedirectToAction("Index");
            }
        }


    }
}
