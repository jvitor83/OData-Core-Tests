using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet.AspNetCore.WebApi.OData.Sample.Models
{

    public class Blog
    {
        public Blog()
        {
            this.Posts = new List<Post>();
        }

        public int BlogId { get; set; }
        public string Url { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; }
    }


}
