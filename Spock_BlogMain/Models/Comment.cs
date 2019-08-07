using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spock_BlogMain.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int BlogMainId { get; set; }
        public string AuthorId { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string UpdateReason { get; set; }
        public virtual BlogMain BlogMain{ get; set; }
        public virtual ApplicationUser Author { get; set; }


    }
}