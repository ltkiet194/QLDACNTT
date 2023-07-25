using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GameStore.Models.DTO
{
    public class CommentNew
    {
        public int Id_Conmment { get; set; }
        public int Id_kh { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public string Content { get; set; }
        public DateTime DateModified { get; set; }
        public List<Reply> ListReply { get; set; }
    }
    public class Reply
    {
            public int Id_Reply { get; set; }
            public int Id_kh { get; set; }
            public string Name { get; set; }
            public string Avatar { get; set; }
            public string Content { get; set; }
            public DateTime DateModified { get; set; }
            public List<CommentNew> ListComment { get; set; }
    }
}