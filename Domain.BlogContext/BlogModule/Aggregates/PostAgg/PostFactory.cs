namespace NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.PostAgg
{
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
