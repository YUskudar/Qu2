﻿using Qu2SM.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Qu2SM.Controllers
{
    public class GroupController : Controller
    {
        private chattingonlyEntities db = new chattingonlyEntities();


        public ActionResult Index()
        {
            return View(); 
        }
        public ActionResult CreateGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateGroup(group group)
        {
            if (ModelState.IsValid)
            {
                Random random = new Random();
                int newGroupCode = random.Next(1000, 1000000000); //grup katılım kodunu rastgele veriyorum.

                var newGroup = new group
                {
                    groupname = group.groupname,
                    groupkatılım = newGroupCode
                };
                db.group.Add(newGroup);
                db.SaveChanges();

                string userId = User.Identity.Name;
                var userGroup = new usergroups
                {
                    user_id = db.user.FirstOrDefault(u => u.useremail == userId).userid,
                    group_id = newGroup.gid
                };
                db.usergroups.Add(userGroup); //burada logları basitleştirmek için kullanıcı ve grubu usergroups modelinde tutuyorum.
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }

            return View(group);
        }

        public ActionResult JoinGroup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinGroup(int? groupCode)
        {
            if (groupCode.HasValue)
            {
                var existingGroup = db.group.FirstOrDefault(g => g.groupkatılım == groupCode);

                if (existingGroup != null)
                {
                    string userId = User.Identity.Name;

                    var userGroup = new usergroups
                    {
                        user_id = db.user.FirstOrDefault(u => u.useremail == userId).userid,
                        group_id = existingGroup.gid
                    };
                    db.usergroups.Add(userGroup);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("groupCode", "Geçersiz grup kodu.");
                }
            }
            else
            {
                ModelState.AddModelError("groupCode", "Grup katılım kodunu girin.");
            }

            return View();
        }

        public ActionResult MyGroups() //Bu kısımda Kullanıcı üyesi olduğu grupları görebiliyor.
        {
            string userId = User.Identity.Name;
            var user = db.user.FirstOrDefault(u => u.useremail == userId);

            if (user != null)
            {
                var userGroups = db.usergroups
                    .Where(ug => ug.user_id == user.userid)
                    .Select(ug => ug.group)
                    .ToList();

                return View(userGroups);
            }
            else
            {
                return RedirectToAction("NotFound", "Error");
            }
        }

        public ActionResult GroupChat()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GroupChat(int groupId) //Bu kısım varolan grupta konuşma yapabileceği kısım.
        {
            bool isUserMemberOfGroup = CheckIfUserIsMemberOfGroup(User.Identity.Name, groupId);

            if (isUserMemberOfGroup)
            {
                var group = db.group.FirstOrDefault(g => g.gid == groupId);
                ViewBag.GroupName = group.groupname;
                ViewBag.Groupdescp = group.groupdescription;
                ViewBag.Groupdavet = group.groupkatılım;

                ViewBag.GroupId = groupId;

                var groupMessages = db.groupmessage // tarihe göre sıralama
                    .Where(m => m.group_id == groupId)
                    .OrderBy(m => m.date)
                    .ToList();

                var groupMembers = db.usergroups
                    .Where(ug => ug.group_id == groupId)
                    .Select(ug => ug.user)
                    .ToList();

                ViewBag.GroupMembers = groupMembers;

                return View(groupMessages);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu gruba üye değilsiniz ve grup sohbetini görüntüleyemezsiniz."); // Kişi başkasından grup linkini alır katılmak isterse hata veriyor.
                return View();
            }
        }



        [HttpPost]
        public ActionResult SendMessage(int groupId, string messageText) //mesaj gönderme kısmı
        {
            bool isUserMemberOfGroup = CheckIfUserIsMemberOfGroup(User.Identity.Name, groupId);

            if (isUserMemberOfGroup)
            {
                var newMessage = new groupmessage
                {
                    gmessage = messageText,
                    date = DateTime.Now,
                    group_id = groupId,
                    user_id = db.user.FirstOrDefault(u => u.useremail == User.Identity.Name).userid //belirli özeliikleri referans alarak atılan mesajın idsini alıyor ve hangi grupta atıldıysa onun idsini alıyor.
                };

                db.groupmessage.Add(newMessage);
                db.SaveChanges();

                return RedirectToAction("GroupChat", new { groupId = groupId });
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu gruba üye değilsiniz ve mesaj gönderemezsiniz.");
                return View(); 
            }
        }
        private bool CheckIfUserIsMemberOfGroup(string userName, int groupId) //Bu kısımda üye o grupta mı değil mi onun kontrolü oluyor.
        {
            using (var db = new chattingonlyEntities())
            {
                var user = db.user.FirstOrDefault(u => u.useremail == userName);
                if (user != null)
                {
                    var userGroup = db.usergroups.FirstOrDefault(ug => ug.user_id == user.userid && ug.group_id == groupId);
                    return userGroup != null; 
                }
                return false; 
            }
        }
        public ActionResult LeaveGroup(int groupId)
        {
            string userId = User.Identity.Name;

            bool isUserMemberOfGroup = CheckIfUserIsMemberOfGroup(userId, groupId);

            if (isUserMemberOfGroup)
            {
                using (var db = new chattingonlyEntities())
                {
                    var user = db.user.FirstOrDefault(u => u.useremail == userId);
                    var userGroup = db.usergroups.FirstOrDefault(ug => ug.user_id == user.userid && ug.group_id == groupId);

                    if (userGroup != null)
                    {
                        db.usergroups.Remove(userGroup);
                        db.SaveChanges();

                        return RedirectToAction("MyGroups");
                    }
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Bu gruptan zaten çıktınız veya üye değilsiniz.");
            }

            return RedirectToAction("GroupChat", new { groupId = groupId });
        }

        public ActionResult GroupMembers(int groupId)
        {
            var groupMembers = db.user
                .Where(u => u.usergroups.Any(ug => ug.group_id == groupId))
                .ToList();

            return View(groupMembers);
        }

 

    }

}
