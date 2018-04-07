using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNet.AspNetCore.WebApi.OData.Sample.Controllers
{
    //[Produces("application/json")]
    //[Route("api/Blogs")]
    public class BlogsController : ODataController
    {
        protected Data.BloggingContext BloggingContext { get; private set; }
        protected IMapper Mapper { get; private set; }


        public BlogsController(Data.BloggingContext bloggingContext, IMapper mapper)
        {
            this.BloggingContext = bloggingContext;
            this.Mapper = mapper;
        }

        //public IActionResult Get(ODataQueryOptions<Data.Blog> oDataQueryOptions)
        //{
        //    var ret = oDataQueryOptions.ApplyTo(this.BloggingContext.Blogs);

        //    return Ok(ret);
        //}

        //    [EnableQuery()]
        //public IQueryable<Data.Blog> Get()
        //{
        //    var ret = this.BloggingContext.Blogs;

        //    return ret;
        //}

        //[EnableQuery()]
        //public IQueryable<Models.Blog> Get()
        //{
        //    //List<Models.Blog> blogs = new List<Models.Blog>();
        //    //blogs.Add(new Models.Blog() {
        //    //    BlogId = 1,
        //    //    Url = "a"
        //    //});
        //    //return blogs.AsQueryable();

        //    var result = this.BloggingContext.Blogs.ProjectTo<Models.Blog>(this.Mapper.ConfigurationProvider);


        //    return result;
        //}


        public IActionResult Get(ODataQueryOptions<Models.Blog> oDataQueryOptions)
        {
            ////Checking that EF does works with groupby (EF 2.1)
            //var checkThatGroupByWorksAtEFCore = this.BloggingContext.Blogs.GroupBy(r => r.BlogId).ToList();


            IQueryable<Data.Blog> blogsData = this.BloggingContext.Blogs;

            ////No Expansion, GroupBy and Aggregate works with AutoMapper
            //var includes = (oDataQueryOptions?.SelectExpand?.RawExpand?.Replace('/', '.')?.Split(',') ?? new string[] { });
            //IQueryable<Models.Blog> blogsApi = blogsData.ProjectTo<Models.Blog>(this.Mapper.ConfigurationProvider, null, includes);

            ////Expansion Works but no GroupBy and Aggregate
            IQueryable<Models.Blog> blogsApi = blogsData.Select(r => new Models.Blog()
            {
                BlogId = r.BlogId,
                Url = r.Url,
                Posts = r.Posts.Select(p => new Models.Post()
                {
                    PostId = p.PostId,
                    BlogId = p.BlogId,
                    Title = p.Title,
                    Content = p.Content,
                })
            });


            ////If i execute here, Expand, GroupBy and Aggregate works 
            ////(but it shouldn't be required since it must apply the options at database, otherwise it will bring all data in memory to apply the ODataQueryOptions)
            //blogsApi = blogsApi.ToList().AsQueryable();

            IQueryable blogsTransformed = oDataQueryOptions.ApplyTo(blogsApi);


            ////This return (without execute) generate the `@odata.context` and `value` at json
            return Ok(blogsTransformed);

            ////This return (by executing the iqueryable) generate the json only with the result (without the `@odata.context` metadata and `value`)
            //IQueryable<object> blogsToExecute = blogsTransformed as IQueryable<object>;
            //var executed = blogsToExecute.ToList();
            //return this.Ok(executed);

        }

    }
}