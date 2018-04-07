using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNet.AspNetCore.WebApi.OData.Sample.Mapper
{
    public class ApiProfile : Profile
    {
        public ApiProfile()
        {
            var blogMapping = CreateMap<Data.Blog, Models.Blog>();
            //blogMapping.ForAllMembers(ExplicitExpansionForEnumerables<Models.Blog, Data.Blog>());
            blogMapping.PreserveReferences();
            blogMapping = blogMapping.MaxDepth(int.MaxValue);
            blogMapping = blogMapping.ForMember(dataBlog => dataBlog.BlogId, dataBlog => dataBlog.MapFrom(modelBlog => modelBlog.BlogId));
            blogMapping = blogMapping.ForMember(dataBlog => dataBlog.Url, dataBlog => dataBlog.MapFrom(modelBlog => modelBlog.Url));
            blogMapping = blogMapping.ForMember(dataBlog => dataBlog.Posts, dataBlog =>
            {
                //dataBlog.Ignore();
                dataBlog.ExplicitExpansion();
                dataBlog.MapFrom(modelBlog => modelBlog.Posts);
            });
            blogMapping.ReverseMap();


            var postMapping = CreateMap<Models.Post, Data.Post>();
            //postMapping.ForAllMembers(ExplicitExpansionForEnumerables<Models.Post, Data.Post>());
            postMapping.PreserveReferences();
            postMapping = postMapping.MaxDepth(int.MaxValue);
            postMapping = postMapping.ForMember(dataPost => dataPost.BlogId, dataPost => dataPost.MapFrom(modelPost => modelPost.BlogId));
            postMapping = postMapping.ForMember(dataPost => dataPost.Content, dataPost => dataPost.MapFrom(modelPost => modelPost.Content));
            postMapping = postMapping.ForMember(dataPost => dataPost.PostId, dataPost => dataPost.MapFrom(modelPost => modelPost.PostId));
            postMapping = postMapping.ForMember(dataPost => dataPost.Title, dataPost => dataPost.MapFrom(modelPost => modelPost.Title));
            postMapping = postMapping.ForMember(dataPost => dataPost.Blog, dataPost =>
            {
                //dataPost.Ignore();
                dataPost.ExplicitExpansion();
                dataPost.MapFrom(modelPost => modelPost.Blog);
            });
            postMapping.ReverseMap();


            this.AllowNullCollections = true;
            this.AllowNullDestinationValues = true;

        }

        //private static Action<IMemberConfigurationExpression<T1, T2, object>> ExplicitExpansionForEnumerables<T1,T2>()
        //{
        //    return r =>
        //    {
        //        var dm = r.DestinationMember as System.Reflection.PropertyInfo;
        //        if (dm != null)
        //        {
        //            if (typeof(System.Collections.IEnumerable).IsAssignableFrom(dm.PropertyType) && dm.PropertyType != typeof(string))
        //            {
        //                r.ExplicitExpansion();
        //            }
        //        }
        //    };
        //}
    }

}
