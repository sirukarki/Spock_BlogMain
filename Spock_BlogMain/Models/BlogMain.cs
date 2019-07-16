﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Spock_BlogMain.Models
{
    public class BlogMain
    {
        public int Id { get; set; }
        
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset ? Updated {get;set;}
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Body { get; set; }
        public string Slug { get; set; }
        public string MediaUrl { get; set; }
        public bool Published { get; set; }
        //virtual nav section
        public virtual ICollection<Comment>Comments { get; set; }

        public BlogMain()
        {
            Comments = new HashSet<Comment>();
        }
    }
}