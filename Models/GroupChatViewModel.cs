using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace chatting.Models
{
    public class GroupChatViewModel
    {
        public group Group { get; set; }
        public List<groupmessage> Messages { get; set; }
        public string NewMessage { get; set; }
    }
}