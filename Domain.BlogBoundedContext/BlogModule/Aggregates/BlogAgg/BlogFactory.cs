namespace NLayerApp.Domain.BlogBoundedContext.BlogModule.Aggregates.BlogAgg
{
    public static class BlogFactory
    {
        public static Blog CreateBlog(string name, string url, int rating)
        {
            Blog blog = new Blog();
            blog.Name = name;
            blog.Url = url;
            blog.Rating = rating;
            return blog;
        }
    }
}
