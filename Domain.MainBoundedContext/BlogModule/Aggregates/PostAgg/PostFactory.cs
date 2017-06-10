namespace NLayerApp.Domain.MainBoundedContext.Aggregates.PostAgg
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    public static class PostFactory
    {
        public static Post CreatePost(string title, string content, int blogId)
        {
            Post post = new Post();
            post.Title = title;
            post.Content = content;
            post.BlogId = blogId;
            return post;
        }
    }
}
